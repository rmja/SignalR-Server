// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.SignalR.Hubs
{
    public class DefaultHubActivator : IHubActivator
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public DefaultHubActivator(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if (descriptor.HubType == null)
            {
                return null;
            }

            return ActivatorUtilities.CreateInstance(_contextAccessor.HttpContext.RequestServices, descriptor.HubType) as IHub ?? ActivatorUtilities.CreateInstance(_serviceProvider, descriptor.HubType) as IHub;
        }
    }
}
