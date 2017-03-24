using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Core.Interfaces.IFactories;
using Infrastructure.Core.Interfaces.IManagers;
using Infrastructure.Core.Interfaces.IServices;
using Infrastructure.Core.Interfaces.ISettings;
using Infrastructure.Models.Constants;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Core.Managers
{
    /// <summary>
    /// File Service + Caching
    /// Should get value from caching first. if not found in cache then get value from file.
    /// if cache manager throws exception then Log it and continue. fall back on file.
    /// if file not found then throw exception
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly ILogger<FileManager> _logger;
        private readonly IFileService _fileService;
        private readonly ICacheManager _cacheManager;
        private readonly ICacheSettingsFactory _cacheSettingsFactory;
        private ICacheSettings _cacheSettings;
        public FileManager(ILogger<FileManager> logger, ICacheManager cacheManager, IFileService fileService, ICacheSettingsFactory cacheSettingsFactory)
        {
            _logger = logger;
            _fileService = fileService;
            _cacheSettingsFactory = cacheSettingsFactory;
            _cacheManager = cacheManager;
            _cacheSettingsFactory = cacheSettingsFactory;
        }

        public async Task<T> ReadXml<T>(string fileName)
        {
            var result = default(T);

            _cacheSettings = _cacheSettingsFactory.GetCacheSettings(CacheTypes.FileSystem, fileName);
            if (_cacheSettings.IsEnabled)
                result = await _cacheManager.Get<T>(_cacheSettings.KeyPrefix);

            if (result == null)
            {
                result = await _fileService.ReadXml<T>(fileName);

                if (_cacheSettings.IsEnabled)
                    await _cacheManager.Set(_cacheSettings.KeyPrefix, result.ToString());
            }

            return result;
        }

        public async Task WriteXml<T>(string fileName, T obj)
        {
            await _fileService.WriteXml(fileName, obj);
        }

        public async Task<string> ReadText(string fileName)
        {
            var result = string.Empty;

            _cacheSettings = _cacheSettingsFactory.GetCacheSettings(CacheTypes.FileSystem, fileName);
            if (_cacheSettings.IsEnabled)
                result = await _cacheManager.Get<string>(_cacheSettings.KeyPrefix);

            if (result == null)
            {
                result = await _fileService.ReadText(fileName);

                if (_cacheSettings.IsEnabled)
                    await _cacheManager.Set(_cacheSettings.KeyPrefix, result.ToString());
            }

            return result;
        }

        public async Task WriteText(string fileName, string txt)
        {
            await _fileService.WriteText(fileName, txt);
        }

        public async Task<List<string[]>> ReadCsv(string fileName)
        {
           return  await _fileService.ReadCsv(fileName);
        }

        public async Task WriteCsv(string fileName, List<string[]> list)
        {
            await _fileService.WriteCsv(fileName, list);
        }
    }
}
