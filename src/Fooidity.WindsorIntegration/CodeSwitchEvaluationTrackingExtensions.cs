namespace Fooidity
{
    using System;
    using System.Collections.Generic;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Contracts;


    public static class CodeSwitchEvaluationTrackingExtensions
    {
        public static void EnableCodeSwitchTracking(this IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel,true));
            container.Register(
                Component.For<ICodeSwitchesEvaluated, IObserver<CodeSwitchEvaluated>>()
                .ImplementedBy<CodeSwitchEvaluationObserver>()
                .LifeStyle.Scoped() //instance per lifetime scope
                //.LifestyleSingleton()
                );
        }

        public static IEnumerable<CodeSwitchEvaluated> GetCodeSwitchesEvaluated(this IWindsorContainer container)
        {
            if (container.Kernel.HasComponent(typeof(ICodeSwitchesEvaluated)))
            {
                return container.Resolve<ICodeSwitchesEvaluated>();
            }

            throw new FooidityException(
                "Code switch tracking is not enabled. Enable it while building the container using EnableCodeSwitchTracking");
        } 
    }
}