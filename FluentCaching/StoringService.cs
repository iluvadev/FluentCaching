﻿using System.Threading.Tasks;
using FluentCaching.Configuration;
using FluentCaching.Exceptions;

namespace FluentCaching
{
    internal class StoringService<T>
        where T : class
    {
        private readonly CachingConfigurationBase _configuration;

        public StoringService(CachingConfigurationBase configuration)
        {
            _configuration = configuration;
        }

        public Task StoreAsync(T targetObject)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetStoreKey(targetObject);

            var implementation = GetCacheImplementation(item);

            return implementation.SetAsync(key, targetObject, item.Options);
        }

        public Task<T> RetrieveAsync(object targetObject)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);

            var implementation = GetCacheImplementation(item);

            return implementation.GetAsync<T>(key);
        }

        public Task<T> RetrieveAsync(string targetString)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetRetrieveKeySimple(targetString);

            var implementation = GetCacheImplementation(item);

            return implementation.GetAsync<T>(key);
        }

        public Task RemoveAsync(object targetObject)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);

            var implementation = GetCacheImplementation(item);

            return implementation.RemoveAsync(key);
        }

        public Task RemoveAsync(string targetString)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetRetrieveKeySimple(targetString);

            var implementation = GetCacheImplementation(item);

            return implementation.RemoveAsync(key);
        }

        private CachingConfigurationItem<T> GetConfigurationItem()
        {
            var item = _configuration.GetItem<T>();

            if (item == null)
            {
                throw new ConfigurationNotFoundException(typeof(T));
            }

            return item;
        }

        private ICacheImplementation GetCacheImplementation(CachingConfigurationItem<T> item) =>
            item.Options.CacheImplementation ??
            _configuration.Current ?? 
            throw new ConfigurationNotFoundException(typeof(T));
    }
}
