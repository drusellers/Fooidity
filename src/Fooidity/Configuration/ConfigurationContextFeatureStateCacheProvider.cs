namespace Fooidity.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Caching;


    public class ConfigurationContextFeatureStateCacheProvider<TContext> :
        IContextFeatureStateCacheProvider<TContext>
    {
        public async Task<IEnumerable<Tuple<string, ICachedCodeFeatureState>>> Load()
        {
            var results = new List<Tuple<string, ICachedCodeFeatureState>>();

            var configuration = ConfigurationManager.GetSection("fooidity") as FooidityConfiguration;
            if (configuration != null)
            {
                if (configuration.Contexts != null)
                {
                    for (int i = 0; i < configuration.Contexts.Count; i++)
                    {
                        ContextElement context = configuration.Contexts[i];

                        var contextId = new ContextId(context.Id);

                        Type contextType = contextId.GetType(false);
                        if (contextType == null)
                            throw new ConfigurationErrorsException("The context type is not valid: " + context.Id);

                        if (contextType != typeof(TContext))
                            continue;

                        if (context.Instances != null)
                        {
                            for (int instanceIndex = 0; instanceIndex < context.Instances.Count; instanceIndex++)
                            {
                                ContextInstanceElement instance = context.Instances[instanceIndex];

                                if (instance.Features != null)
                                {
                                    for (int j = 0; j < instance.Features.Count; j++)
                                    {
                                        FeatureStateElement feature = instance.Features[j];

                                        var featureId = new CodeFeatureId(feature.Id);

                                        Type codeFeatureType = featureId.GetType(false);
                                        if (codeFeatureType == null)
                                            throw new ConfigurationErrorsException("The feature type is not valid: " + feature.Id);

                                        ICachedCodeFeatureState codeState = new CachedCodeFeatureState(featureId, feature.Enabled);

                                        results.Add(Tuple.Create(instance.Key, codeState));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}