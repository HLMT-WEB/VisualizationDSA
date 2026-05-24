using System;
using System.Threading.Tasks;

namespace VisualizationDSA.Application.Services
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
        bool TryGet<T>(string key, out T? value);
    }
}
