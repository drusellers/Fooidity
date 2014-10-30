namespace Fooidity
{
    using Caching;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Configuration;


    /// <summary>
    /// Registers the types required to resolve the context cache for a given context type
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TKeyProvider"></typeparam>
    public class ConfigurationContextFeatureCacheInstaller<TContext, TKeyProvider> :
        IWindsorInstaller
        where TKeyProvider : class, ContextKeyProvider<TContext>
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ContextKeyProvider<TContext>>().ImplementedBy<TKeyProvider>(),
                Component.For<IContextFeatureStateCache<TContext>>().ImplementedBy<ContextFeatureStateCache<TContext>>()
                    .Named("contextFeatureCache").LifestyleSingleton(),
                Component.For<IContextFeatureStateCacheProvider<TContext>>().ImplementedBy<ConfigurationContextFeatureStateCacheProvider<TContext>>()
                );
        }
    }


}