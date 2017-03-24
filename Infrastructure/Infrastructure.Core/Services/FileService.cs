using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Infrastructure.Core.Interfaces.IServices;

namespace Infrastructure.Core.Services
{
    public class FileService : IFileService
    {
        public async Task<T> ReadXml<T>(string fileName)
        {
            return await Task.Factory.StartNew(() =>
            {

                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var reader = XmlReader.Create(fs))
                    {
                        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                        var obj = serializer.Deserialize(reader);
                        return (T)obj;
                    }
                }
            });

        }

        public Task WriteXml<T>(string fileName, T obj)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadText(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task WriteText(string fileName, string txt)
        {
            throw new NotImplementedException();
        }

        public Task<List<string[]>> ReadCsv(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task WriteCsv(string fileName, List<string[]> list)
        {
            throw new NotImplementedException();
        }
    }
}
