﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Parameters;

namespace FluentCaching.Benchmarks
{
    public class DictionaryImplementation : ICacheImplementation
    {
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult((T)_dictionary[key]);
        }

        public Task SetAsync<T>(string key, T targetObject, CachingOptions options)
        {
            _dictionary[key] = targetObject;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _dictionary.Remove(key);
            return Task.CompletedTask;
        }

    }
}
