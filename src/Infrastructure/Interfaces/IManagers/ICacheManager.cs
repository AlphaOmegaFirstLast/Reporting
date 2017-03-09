using System.Threading.Tasks;

namespace Infrastructure.Interfaces.IManagers
{
    /// <summary>
    /// Caching Service + Serialization 
    /// </summary>
    public interface ICacheManager 
    {
        Task<T> Get<T>(string key);
        Task Set<T>(string key, T value);
        void Remove(string key);
    }
}
