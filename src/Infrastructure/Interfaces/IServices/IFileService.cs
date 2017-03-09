using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.IServices
{
    public interface IFileService
    {
        Task<T> ReadXml<T>(string fileName);
        Task WriteXml<T>(string fileName, T obj);
        Task<string> ReadText(string fileName);
        Task WriteText(string fileName, string txt);
        Task<List<string[]>> ReadCsv(string fileName);
        Task WriteCsv(string fileName, List<string[]> list);
    }
}
