using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class StringKeyRetrieveStrategy<T> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<T>
    where T : class
{
    public StringKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }
    
    public Task<T> RetrieveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromStringKey(source.StringKey);
        return GetCacheImplementation<T>(item)
            .RetrieveAsync<T>(key);    
    }
}