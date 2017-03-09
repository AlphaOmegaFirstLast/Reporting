using System.Threading.Tasks;

namespace Infrastructure.Interfaces.IServices
{
    public interface ICacheService
    {
        object Get(string key);
        void Set(string key, string value);
        void Remove(string key);
    }
}
