using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Interfaces.ISettings;

namespace Infrastructure.Services
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
