using System.Threading.Tasks;

namespace Infrastructure.Core.Interfaces.IServices
{
    public interface ISerializationService
    {
       Task<T> Deserialize<T>(string json);
       Task<string> Serialize<T>(T obj);
    }
}
