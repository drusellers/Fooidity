﻿namespace Fooidity.Caching
{
    using System;
    using Configuration;
    using Internals;


    class CodeFeatureStateCacheInstance :
        ICodeFeatureStateCacheInstance
    {
        readonly ICache<CodeFeatureId, CodeFeatureState> _cache;
        readonly bool _defaultState;
        readonly ICacheIndex<Type, CodeFeatureState> _typeIndex;

        public CodeFeatureStateCacheInstance(ICache<CodeFeatureId, CodeFeatureState> cache,
            ICacheIndex<Type, CodeFeatureState> typeIndex, bool defaultState)
        {
            _cache = cache;
            _typeIndex = typeIndex;
            _defaultState = defaultState;
        }

        public bool DefaultState
        {
            get { return _defaultState; }
        }

        public int Count
        {
            get { return _cache.Values.Count; }
        }

        public bool TryGetState(CodeFeatureId id, out CodeFeatureState featureState)
        {
            return _cache.TryGet(id, out featureState);
        }

        public bool TryUpdate(CodeFeatureId id, CodeFeatureState featureState, CodeFeatureState previousFeatureState)
        {
            return _cache.TryUpdate(id, featureState, previousFeatureState);
        }
    }
}