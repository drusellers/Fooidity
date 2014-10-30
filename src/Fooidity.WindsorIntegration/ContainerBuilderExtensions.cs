namespace Fooidity
{
    using System;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;


    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Register a factory method that is switched by the specified code feature
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="builder">The container builder</param>
        /// <param name="enabledFactory">Factory to use when switch is enabled</param>
        /// <param name="disabledFactory">Factory to use when switch is disabled</param>
        /// <returns></returns>
        public static void RegisterSwitched<TFeature, T>(
            this IWindsorContainer builder, Func<IKernel, T> enabledFactory, Func<IKernel, T> disabledFactory)
            where TFeature : struct, CodeFeature
            where T : class
         
        {
            builder.Register(Component.For<T>().UsingFactoryMethod(k =>
            {
                var codeSwitch = k.Resolve<CodeSwitch<TFeature>>();
                return codeSwitch.Enabled
                    ? enabledFactory(k)
                    : disabledFactory(k);

            }));
        }

        /// <summary>
        /// Register two types that are selectively resolved depending upon the state of the CodeSwitch
        /// </summary>
        /// <typeparam name="TFeature">The CodeSwitch type</typeparam>
        /// <typeparam name="T">The registration type</typeparam>
        /// <typeparam name="TEnabled">The enabled type</typeparam>
        /// <typeparam name="TDisabled">The disable type</typeparam>
        /// <param name="builder">The container builder</param>
        /// <returns>The registration builder for the container, already configured for the specified types</returns>
        public static void RegisterSwitchedType
            <TFeature, T, TEnabled, TDisabled>(this IWindsorContainer builder)
            where TFeature : struct, CodeFeature
            where T : class
            where TEnabled : class, T
            where TDisabled : class, T
        {
            builder.Register(
                Component.For<TEnabled>().ImplementedBy<TEnabled>(),
                Component.For<TDisabled>().ImplementedBy<TDisabled>(),
                Component.For<T>().UsingFactoryMethod(kernel =>
                {
                    var codeSwitch = kernel.Resolve<CodeSwitch<TFeature>>();

                    return codeSwitch.Enabled ? (T)kernel.Resolve<TEnabled>() : (T)kernel.Resolve<TDisabled>();

                })
                );
        }
    }
}