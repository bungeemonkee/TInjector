using System;
using System.Collections.Generic;

namespace TInjector.Locator
{
    public class RootLocatorGetArguments
    {
        /// <summary>
        /// The request that required this call.
        /// </summary>
        public readonly IRequest Request;

        /// <summary>
        /// The cache of graph-scope objects for this call.
        /// </summary>
        public readonly IDictionary<Type, object> GraphCache;

        /// <summary>
        /// The collection of activation callbacks for this object graph.
        /// </summary>
        public readonly IList<Action> ActivationCallbacks;

        public RootLocatorGetArguments(IRequest request, IDictionary<Type, object> graphCache, IList<Action> activationCallbacks)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (graphCache == null)
            {
                throw new ArgumentNullException(nameof(graphCache));
            }

            if (activationCallbacks == null)
            {
                throw new ArgumentNullException(nameof(activationCallbacks));
            }

            Request = request;
            GraphCache = graphCache;
            ActivationCallbacks = activationCallbacks;
        }

        public RootLocatorGetArguments(RootLocatorGetArguments copy)
        {
            Request = copy.Request;
            GraphCache = copy.GraphCache;
            ActivationCallbacks = copy.ActivationCallbacks;
        }
    }
}
