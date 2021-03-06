﻿// TInjector: TInjector
// ScopeType.cs
// Created: 2015-10-17 5:54 PM

namespace TInjector
{
    /// <summary>
    ///     Represents the supported scope types.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        ///     A new instance is created each time the service is requested.
        /// </summary>
        Transient = 0,

        /// <summary>
        ///     A new instance is created once per root, after that no new instances are ever created by that root.
        /// </summary>
        Singleton = 1,

        /// <summary>
        ///     A new instance is created for each object graph.
        ///     The instance will be shared by all objects created as a result of the call to the root.
        /// </summary>
        Graph = 2
    }
}