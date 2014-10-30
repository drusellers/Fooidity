namespace Fooidity
{
    using Caching;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Configuration;
    using Events;


    public class ConfigurationCodeFeatureCacheInstaller : 
        IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ICodeFeatureStateCache, IUpdateCache<CodeFeatureStateUpdated>,IReloadCache>()
                .ImplementedBy<CodeFeatureStateCache>()
                .Named("codeFeatureCache")
                .LifestyleSingleton(),

                Component.For<ICodeFeatureStateCacheProvider>().ImplementedBy<ConfigurationCodeFeatureStateCacheProvider>()

                );
        }
    }
}