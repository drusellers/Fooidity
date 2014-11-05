﻿namespace Fooidity.Contracts
{
    using System;


    /// <summary>
    /// Observable when the state cache is loaded
    /// </summary>
    public interface IContextCodeFeatureStateCacheLoaded
    {
        /// <summary>
        /// Identifies the event
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The time the cache load started
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The time taken to load the cache
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// The number of context instances
        /// </summary>
        int ContextCount { get; }

        /// <summary>
        /// The host that loaded the cache
        /// </summary>
        IHost Host { get; }
    }
}