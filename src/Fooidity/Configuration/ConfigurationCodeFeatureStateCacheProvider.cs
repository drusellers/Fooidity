﻿namespace Fooidity.Configuration
{
    using System;
    using System.Configuration;
    using Caching;
    using Caching.Internals;


    public class ConfigurationCodeFeatureStateCacheProvider :
        ICodeFeatureStateCacheProvider
    {
        public ICodeFeatureStateCacheInstance Load()
        {
            var cache = new InMemoryCache<CodeFeatureId, CodeFeatureState>();
            ICacheIndex<Type, CodeFeatureState> index = cache.GetIndex(x => x.FeatureType);
            bool defaultState = false;

            var configuration = ConfigurationManager.GetSection("fooidity") as FooidityConfiguration;
            if (configuration != null)
            {
                defaultState = configuration.DefaultState;

                if (configuration.Features != null)
                {
                    for (int i = 0; i < configuration.Features.Count; i++)
                    {
                        FeatureStateElement feature = configuration.Features[i];

                        var featureId = new CodeFeatureId(feature.Id);

                        Type codeFeatureType = featureId.GetType(false);
                        if (codeFeatureType == null)
                            throw new ConfigurationErrorsException("The feature type is not valid: " + feature.Id);

                        var state = new FeatureState(featureId, codeFeatureType, feature.Enabled);

                        cache.TryAdd(state.Id, state);
                    }
                }
            }

            return new CodeFeatureStateCacheInstance(cache, index, defaultState);
        }


        class FeatureState :
            CodeFeatureState
        {
            readonly bool _enabled;
            readonly Type _featureType;
            readonly CodeFeatureId _id;

            public FeatureState(CodeFeatureId id, Type featureType, bool enabled)
            {
                _enabled = enabled;
                _featureType = featureType;
                _id = id;
            }

            public CodeFeatureId Id
            {
                get { return _id; }
            }

            public Type FeatureType
            {
                get { return _featureType; }
            }

            public bool Enabled
            {
                get { return _enabled; }
            }
        }
    }
}