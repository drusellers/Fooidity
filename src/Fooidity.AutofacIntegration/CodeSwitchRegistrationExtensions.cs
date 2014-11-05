namespace Fooidity
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using CodeSwitches;
    using Contracts;


    public static class CodeSwitchRegistrationExtensions
    {
        /// <summary>
        /// By default, all code switches that are not explicitly registered will use a default implementation
        /// that is disabled.
        /// </summary>
        /// <param name="builder"></param>
        public static void CodeSwitchesDisabledbyDefault(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DisabledCodeSwitch<>))
                .As(typeof(ICodeSwitch<>))
                .OnActivating(x => OnCodeSwitchActivation(x.Context, (IObservable<ICodeSwitchEvaluated>)x.Instance));
        }

        /// <summary>
        /// By default, all code switches that are not explicitly registered will use a default implementation
        /// that is enabled.
        /// </summary>
        /// <param name="builder"></param>
        public static void CodeSwitchesEnabledByDefault(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EnabledCodeSwitch<>))
                .As(typeof(ICodeSwitch<>))
                .OnActivating(x => OnCodeSwitchActivation(x.Context, (IObservable<ICodeSwitchEvaluated>)x.Instance));
        }

        /// <summary>
        /// Register the specified CodeSwitch as enabled in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="builder">The container builder to register</param>
        public static void RegisterCodeSwitchEnabled<TFeature>(this ContainerBuilder builder)
            where TFeature : struct, ICodeFeature
        {
            builder.RegisterType<EnabledCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance));
        }

        /// <summary>
        /// Register the specified CodeSwitch as disabled in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="builder">The container builder to register</param>
        public static void RegisterCodeSwitchDisabled<TFeature>(this ContainerBuilder builder)
            where TFeature : struct, ICodeFeature
        {
            builder.RegisterType<DisabledCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance));
        }

        /// <summary>
        /// Enable the CodeSwitch in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container to update</param>
        public static void EnableCodeSwitch<TFeature>(this IContainer container)
            where TFeature : struct, ICodeFeature
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EnabledCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .OnActivated(x => OnCodeSwitchActivation(x.Context, x.Instance))
                .SingleInstance();

            builder.Update(container);
        }

        /// <summary>
        /// Disable the CodeSwitch in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container to update</param>
        public static void DisableCodeSwitch<TFeature>(this IContainer container)
            where TFeature : struct, ICodeFeature
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DisabledCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance))
                .SingleInstance();

            builder.Update(container);
        }

        /// <summary>
        /// Registers a toggle switch with a shared toggle state 
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <param name="builder">The container builder</param>
        /// <param name="initial">True if the toggle should be enabled initially</param>
        public static void RegisterCodeSwitchToggle<TFeature>(this ContainerBuilder builder, bool initial = false)
            where TFeature : struct, ICodeFeature
        {
            builder.Register(context => new ToggleSwitchState<TFeature>(initial))
                .As<IToggleSwitchState<TFeature>>()
                .SingleInstance();

            builder.RegisterType<ToggleCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .As<IToggleCodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance));
        }

        public static void RegisterCodeSwitch<TFeature>(this ContainerBuilder builder)
            where TFeature : struct, ICodeFeature
        {
            builder.RegisterType<CodeFeatureStateCodeSwitch<TFeature>>()
                .As<ICodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance));
        }

        /// <summary>
        /// Register a context switch for the feature that uses the context to determine if the switch
        /// is enabled for that particular context.
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <typeparam name="TContext">The switch context</typeparam>
        /// <param name="builder"></param>
        /// <param name="throwIfContextNotFound">If the context is not available, throw an exception</param>
        public static void RegisterContextCodeSwitch<TFeature, TContext>(this ContainerBuilder builder, bool throwIfContextNotFound = false)
            where TFeature : struct, ICodeFeature
        {
            builder.Register<ICodeSwitch<TFeature>>(context =>
            {
                // this gives a cleaner error message than a container exception
                TContext switchContext;
                if (!context.TryResolve(out switchContext))
                {
                    if(throwIfContextNotFound)  
                        throw new ContextSwitchException("The context type was not found: " + typeof(TContext).Name);

                    return new CodeFeatureStateCodeSwitch<TFeature>(context.Resolve<ICodeFeatureStateCache>());
                }

                var cache = context.Resolve<ICodeFeatureStateCache>();
                var contextCache = context.Resolve<IContextFeatureStateCache<TContext>>();

                return new ContextFeatureStateCodeSwitch<TFeature, TContext>(cache, contextCache, switchContext);
            })
                .As<ICodeSwitch<TFeature>>()
                .OnActivating(x => OnCodeSwitchActivation(x.Context, x.Instance));
        }

        static void OnCodeSwitchActivation(IComponentContext context, IObservable<ICodeSwitchEvaluated> observable)
        {
            IEnumerable<IObserver<ICodeSwitchEvaluated>> observers;
            if (context.TryResolve(out observers))
            {
                foreach (var observer in observers)
                    observable.Subscribe(observer);
            }
        }
    }
}