namespace Fooidity
{
    using System;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using CodeSwitches;
    using Contracts;


    public static class CodeSwitchRegistrationExtensions
    {
        /// <summary>
        /// By default, all code switches that are not explicitly registered will use a default implementation
        /// that is disabled.
        /// </summary>
        /// <param name="container"></param>
        public static void DisableCodeSwitchesByDefault(this IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof(CodeSwitch<>))
                    .ImplementedBy(typeof(DisabledCodeSwitch<>))
                    .OnCreate((k,t)=>OnCodeSwitchActivation(k,(IObservable<CodeSwitchEvaluated>)t))
                );
        }

        /// <summary>
        /// By default, all code switches that are not explicitly registered will use a default implementation
        /// that is enabled.
        /// </summary>
        /// <param name="container"></param>
        public static void EnableCodeSwitchesByDefault(this IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof(CodeSwitch<>))
                    .ImplementedBy(typeof(EnabledCodeSwitch<>))
                    .OnCreate((k, t) => OnCodeSwitchActivation(k, (IObservable<CodeSwitchEvaluated>)t))
                );
        }

        /// <summary>
        /// Register the specified CodeSwitch as enabled in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container container to register</param>
        public static void RegisterEnabled<TFeature>(this IWindsorContainer container)
            where TFeature : struct, CodeFeature
        {
            container.Register(
                Component.For<CodeSwitch<TFeature>>()
                .ImplementedBy<EnabledCodeSwitch<TFeature>>()
                    .OnCreate(OnCodeSwitchActivation)
                );
        }

        /// <summary>
        /// Register the specified CodeSwitch as disabled in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container container to register</param>
        public static void RegisterDisabled<TFeature>(this IWindsorContainer container)
            where TFeature : struct, CodeFeature
        {
            container.Register(
                Component.For<CodeSwitch<TFeature>>()
                .Named(Guid.NewGuid().ToString())
                .ImplementedBy<DisabledCodeSwitch<TFeature>>()
                    .OnCreate(OnCodeSwitchActivation)
                );
        }

        /// <summary>
        /// Enable the CodeSwitch in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container to update</param>
        public static void Enable<TFeature>(this IWindsorContainer container)
            where TFeature : struct, CodeFeature
        {
            throw new NotSupportedException("Castle Windsor does not support this feature yet");
            container.Register(
                Component.For<CodeSwitch<TFeature>>()
                .ImplementedBy<EnabledCodeSwitch<TFeature>>()
                    .IsDefault()
                    .Named(Guid.NewGuid().ToString())
                    .OnCreate(OnCodeSwitchActivation)
                    .LifestyleSingleton()
                );
        }

        /// <summary>
        /// Disable the CodeSwitch in the container
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <param name="container">The container to update</param>
        public static void Disable<TFeature>(this IWindsorContainer container)
            where TFeature : struct, CodeFeature
        {
            throw new NotSupportedException("Castle Windsor does not support this feature yet");
            container.Register(
               Component.For<CodeSwitch<TFeature>>()
               .ImplementedBy<DisabledCodeSwitch<TFeature>>()
                   .OnCreate(OnCodeSwitchActivation)
                   .LifestyleSingleton()
               );
        }

        /// <summary>
        /// Registers a toggle switch with a shared toggle state 
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <param name="container">The container container</param>
        /// <param name="enabled">True if the toggle should be enabled initially</param>
        public static void RegisterToggle<TFeature>(this IWindsorContainer container, bool enabled = false)
            where TFeature : struct, CodeFeature
        {
            container.Register(
                Component.For<IToggleSwitchState<TFeature>>()
                    .UsingFactoryMethod(_ => new ToggleSwitchState<TFeature>(enabled))
                    .LifestyleSingleton(),

                Component.For<CodeSwitch<TFeature>,IToggleCodeSwitch<TFeature>>()
                    .ImplementedBy<ToggleCodeSwitch<TFeature>>()
                    .LifestyleTransient()
                    .OnCreate(OnCodeSwitchActivation)
                );
        }

        public static void RegisterCodeSwitch<TFeature>(this IWindsorContainer container)
            where TFeature : struct, CodeFeature
        {
            container.Register(
                Component.For<CodeSwitch<TFeature>>()
                .ImplementedBy<CodeFeatureStateCodeSwitch<TFeature>>()
                   .OnCreate(OnCodeSwitchActivation)
                );
        }

        /// <summary>
        /// Register a kernel switch for the feature that uses the kernel to determine if the switch
        /// is enabled for that particular kernel.
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <typeparam name="TContext">The switch kernel</typeparam>
        /// <param name="container"></param>
        /// <param name="throwIfContextNotFound">If the kernel is not available, throw an exception</param>
        public static void RegisterContextSwitch<TFeature, TContext>(this IWindsorContainer container, bool throwIfContextNotFound = true)
            where TFeature : struct, CodeFeature
        {
            container.Register(
                Component.For<CodeSwitch<TFeature>>().UsingFactoryMethod<CodeSwitch<TFeature>>(kernel =>
                {

                    if (!kernel.HasComponent(typeof(TContext)))
                    {
                        if (throwIfContextNotFound)
                            throw new ContextSwitchException("The kernel type was not found: " + typeof(TContext).Name);

                        return new CodeFeatureStateCodeSwitch<TFeature>(kernel.Resolve<ICodeFeatureStateCache>());
                    }

                    var switchContext = kernel.Resolve<TContext>();
                    var cache = kernel.Resolve<ICodeFeatureStateCache>();
                    var contextCache = kernel.Resolve<IContextFeatureStateCache<TContext>>();

                    return new ContextFeatureStateCodeSwitch<TFeature, TContext>(cache, contextCache, switchContext);
                }).OnCreate(OnCodeSwitchActivation)
                );

        }

        static void OnCodeSwitchActivation(IKernel kernel, IObservable<CodeSwitchEvaluated> observable)
        {
            var observers = kernel.ResolveAll<IObserver<CodeSwitchEvaluated>>();

            foreach (var observer in observers)
                observable.Subscribe(observer);
        }
    }
}