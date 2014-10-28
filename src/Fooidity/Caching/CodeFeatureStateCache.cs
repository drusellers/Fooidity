﻿namespace Fooidity.Caching
{
    using System;
    using System.Threading;
    using Configuration;
    using Events;
    using Internals;
    using Metadata;


    /// <summary>
    /// Caches the switch states
    /// </summary>
    public class CodeFeatureStateCache :
        ICodeFeatureStateCache,
        IReloadCache,
        IObservable<CodeFeatureStateCacheLoaded>,
        IObservable<CodeFeatureStateCacheUpdated>,
        IUpdateCache<CodeFeatureStateUpdated>
    {
        readonly Connectable<IObserver<CodeFeatureStateCacheLoaded>> _cacheLoaded;
        readonly ICodeFeatureStateCacheProvider _cacheProvider;
        readonly Connectable<IObserver<CodeFeatureStateCacheUpdated>> _cacheUpdated;
        ICodeFeatureStateCacheInstance _cache;

        public CodeFeatureStateCache(ICodeFeatureStateCacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;

            _cacheLoaded = new Connectable<IObserver<CodeFeatureStateCacheLoaded>>();
            _cacheUpdated = new Connectable<IObserver<CodeFeatureStateCacheUpdated>>();

            _cache = _cacheProvider.Load();
        }

        public bool TryGetState<TFeature>(out CodeFeatureState featureState)
        {
            if (_cache.TryGetState(CodeFeatureMetadata<TFeature>.Id, out featureState))
                return true;

            if (_cache.DefaultState)
            {
                featureState = new DefaultCodeFeatureState<TFeature>(true);
                return true;
            }

            return false;
        }

        public IDisposable Subscribe(IObserver<CodeFeatureStateCacheLoaded> observer)
        {
            return _cacheLoaded.Connect(observer);
        }

        public IDisposable Subscribe(IObserver<CodeFeatureStateCacheUpdated> observer)
        {
            return _cacheUpdated.Connect(observer);
        }

        public void ReloadCache()
        {
            DateTime startTime = DateTime.UtcNow;
            ICodeFeatureStateCacheInstance cache = _cacheProvider.Load();
            DateTime endTime = DateTime.UtcNow;

            Interlocked.Exchange(ref _cache, cache);

            var loaded = new Loaded(startTime, endTime - startTime, cache.Count);

            _cacheLoaded.ForEach(x => x.OnNext(loaded));
        }

        public void UpdateCache(CodeFeatureStateUpdated value)
        {
            CodeFeatureState existingFeatureState;
            var featureId = new CodeFeatureId(value.Id);
            if (_cache.TryGetState(featureId, out existingFeatureState))
            {
                var updatedFeatureState = new UpdatedCodeFeatureState(featureId, value.Enabled);

                DateTime startTime = DateTime.UtcNow;
                bool updated = _cache.TryUpdate(featureId, updatedFeatureState, existingFeatureState);
                DateTime endTime = DateTime.UtcNow;

                if (updated)
                {
                    var updatedEvent = new Updated(startTime, endTime - startTime, updatedFeatureState.Id, updatedFeatureState.Enabled);

                    _cacheUpdated.ForEach(x => x.OnNext(updatedEvent));
                }
            }
        }


        class Loaded :
            CodeFeatureStateCacheLoaded
        {
            readonly int _count;
            readonly TimeSpan _duration;
            readonly DateTime _timestamp;

            public Loaded(DateTime timestamp, TimeSpan duration, int count)
            {
                _timestamp = timestamp;
                _duration = duration;
                _count = count;
            }

            public DateTime Timestamp
            {
                get { return _timestamp; }
            }

            public TimeSpan Duration
            {
                get { return _duration; }
            }

            public int Count
            {
                get { return _count; }
            }
        }


        class Updated :
            CodeFeatureStateCacheUpdated
        {
            readonly TimeSpan _duration;
            readonly bool _enabled;
            readonly Uri _id;
            readonly DateTime _timestamp;

            public Updated(DateTime timestamp, TimeSpan duration, CodeFeatureId id, bool enabled)
            {
                _timestamp = timestamp;
                _duration = duration;
                _id = id;
                _enabled = enabled;
            }

            public DateTime Timestamp
            {
                get { return _timestamp; }
            }

            public TimeSpan Duration
            {
                get { return _duration; }
            }

            public Uri Id
            {
                get { return _id; }
            }

            public bool Enabled
            {
                get { return _enabled; }
            }
        }


        class UpdatedCodeFeatureState :
            CodeFeatureState
        {
            readonly bool _enabled;
            readonly CodeFeatureId _id;

            public UpdatedCodeFeatureState(CodeFeatureId id, bool enabled)
            {
                _id = id;
                _enabled = enabled;
            }

            public CodeFeatureId Id
            {
                get { return _id; }
            }

            public bool Enabled
            {
                get { return _enabled; }
            }
        }
    }
}