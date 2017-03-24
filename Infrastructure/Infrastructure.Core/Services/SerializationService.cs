using System.Threading.Tasks;
using Infrastructure.Core.Interfaces.IServices;
using Infrastructure.Core.Interfaces.ISettings;
using Newtonsoft.Json;

namespace Infrastructure.Core.Services
{
    public class SerializationService : ISerializationService
    {
        private readonly ISerializationSettings _serializationSettings;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SerializationService()
        {
            _jsonSerializerSettings = null;
            _serializationSettings = null;
        }

        public async Task<T> Deserialize<T>(string json)
        {
            return await Task.Factory.StartNew(()=> JsonConvert.DeserializeObject<T>(json , _jsonSerializerSettings))   ;
        }

        public async Task<string> Serialize<T>(T obj)
        {
            return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
        }
    }
}
