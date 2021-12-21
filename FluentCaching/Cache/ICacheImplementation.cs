﻿using FluentCaching.Cache.Models;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    public interface ICacheImplementation
    {
        Task<T> RetrieveAsync<T>(string key);

        Task CacheAsync<T>(string key, T targetObject, CacheOptions options);

        Task RemoveAsync(string key);
    }
}
