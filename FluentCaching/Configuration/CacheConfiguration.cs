﻿using System;
using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;

namespace FluentCaching.Configuration
{
    internal sealed class CacheConfiguration : ICacheConfiguration
    {
        private readonly Dictionary<Type, ICacheConfigurationItem> _predefinedConfigurations
            = //Should be thread safe when readonly (mutations present only at configuration phase)
            new Dictionary<Type, ICacheConfigurationItem>();

        private ICacheImplementation _cacheImplementation;

        public ICacheImplementation Current => _cacheImplementation;

        public ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation)
        {
            _cacheImplementation = cacheImplementation;
            return this;
        }

        public ICacheConfiguration For<T>(Func<CachingKeyBuilder<T>, ExpirationTypeBuilder> factoryFunc)
            where T : class
            => For<T>(factoryFunc(new CachingKeyBuilder<T>()).CachingOptions);

        public ICacheConfiguration For<T>(Func<CachingKeyBuilder<T>, CacheImplementationBuilder> factoryFunc)
            where T : class
            => For<T>(factoryFunc(new CachingKeyBuilder<T>()).CachingOptions);

        public ICacheConfigurationItem<T> GetItem<T>() where T : class =>
            _predefinedConfigurations.TryGetValue(typeof(T), out var configurationItem)
                ? configurationItem as CacheConfigurationItem<T>
                : null;

        private ICacheConfiguration For<T>(CacheOptions options)
            where T : class
        {
            _predefinedConfigurations[typeof(T)] = new CacheConfigurationItem<T>(options);
            return this;
        }
    }
}
