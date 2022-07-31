﻿using System;
using FluentCaching.Cache;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfiguration
    {
        ICacheImplementation Current { get; }

        ICacheConfigurationItem<T> GetItem<T>()
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc)
            where T : class;

        ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation);
    }
}