// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Maps SignalR hubs to the app builder pipeline at "/signalr".
        /// </summary>
        /// <param name="builder">The app builder</param>
        public static IBuilder UseSignalR(this IBuilder builder)
        {
            return builder.UseSignalR("/signalr");
        }

        /// <summary>
        /// Maps SignalR hubs to the app builder pipeline at the specified path.
        /// </summary>
        /// <param name="builder">The app builder</param>
        /// <param name="path">The path to map signalr hubs</param>
        public static IBuilder UseSignalR(this IBuilder builder, string path)
        {
            return builder.Map(path, subApp => subApp.RunSignalR());
        }

        /// <summary>
        /// Adds SignalR hubs to the app builder pipeline.
        /// </summary>
        /// <param name="builder">The app builder</param>
        public static void RunSignalR(this IBuilder builder)
        {
            builder.RunSignalR(typeof(HubDispatcher));
        }

        /// <summary>
        /// Maps the specified SignalR <see cref="PersistentConnection"/> to the app builder pipeline at 
        /// the specified path.
        /// </summary>
        /// <typeparam name="TConnection">The type of <see cref="PersistentConnection"/></typeparam>
        /// <param name="builder">The app builder</param>
        /// <param name="path">The path to map the persistent connection</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is syntactic sugar")]
        public static IBuilder UseSignalR<TConnection>(this IBuilder builder, string path) where TConnection : PersistentConnection
        {
            return builder.UseSignalR(path, typeof(TConnection));
        }

        /// <summary>
        /// Maps the specified SignalR <see cref="PersistentConnection"/> to the app builder pipeline at 
        /// the specified path.
        /// </summary>
        /// <param name="builder">The app builder</param>
        /// <param name="path">The path to map the persistent connection</param>
        /// <param name="connectionType">The type of <see cref="PersistentConnection"/></param>
        public static IBuilder UseSignalR(this IBuilder builder, string path, Type connectionType)
        {
            return builder.Map(path, subApp => subApp.RunSignalR(connectionType));
        }

        /// <summary>
        /// Adds the specified SignalR <see cref="PersistentConnection"/> to the app builder.
        /// </summary>
        /// <typeparam name="TConnection">The type of <see cref="PersistentConnection"/></typeparam>
        /// <param name="builder">The app builder</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is syntactic sugar")]
        public static void RunSignalR<TConnection>(this IBuilder builder) where TConnection : PersistentConnection
        {
            builder.RunSignalR(typeof(TConnection));
        }

        /// <summary>
        /// Adds the specified SignalR <see cref="PersistentConnection"/> to the app builder.
        /// </summary>
        /// <param name="builder">The app builder</param>
        /// <param name="connectionType">The type of <see cref="PersistentConnection"/></param>
        public static void RunSignalR(this IBuilder builder, Type connectionType)
        {
            builder.UseMiddleware<PersistentConnectionMiddleware>(connectionType);
        }
    }
}
