using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Infrastructure.Interfaces.IFactories;
using Infrastructure.Interfaces.IManagers;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Interfaces.ISettings;
using Infrastructure.Managers;
using Infrastructure.Models.Constants;
using Moq;
using NUnit.Framework;

namespace Infrastructure.UnitTests.Managers
{
    [TestFixture]
    public class FileManagerTest
    {
        private readonly Mock<ILogger<FileManager>> _loggerMock = new Mock<ILogger<FileManager>>();
        private readonly Mock<ICacheSettingsFactory> _cacheSettingsFactoryMock = new Mock<ICacheSettingsFactory>();
        private readonly Mock<ICacheManager> _cacheManagerMock = new Mock<ICacheManager>();
        private readonly Mock<IFileService> _fileServiceMock = new Mock<IFileService>();
        private readonly Mock<ICacheSettings> _cacheSettingsMock = new Mock<ICacheSettings>();

        /// <summary>
        /// try to get object from cache. set up cache mock return a value and the fileService to return null.
        /// </summary>
        [Test]
        public async Task Should_SuccessGetFromCache()
        {
            _cacheSettingsMock.Setup(x => x.IsEnabled).Returns(true);
            _cacheSettingsFactoryMock.Setup(x => x.GetCacheSettings(CacheTypes.FileSystem, It.IsAny<string>())).Returns(_cacheSettingsMock.Object);
            _cacheManagerMock.Setup(x => x.Get<TestUser>(It.IsAny<string>())).Returns(Task.FromResult(new TestUser()));
            _fileServiceMock.Setup(x=> x.ReadXml<TestUser>(It.IsAny<string>())).Returns(Task.FromResult<TestUser>(null));

            var fileManager = new FileManager(_loggerMock.Object, _cacheManagerMock.Object, _fileServiceMock.Object, _cacheSettingsFactoryMock.Object);
            var result = await fileManager.ReadXml<TestUser>("");
            Assert.NotNull(result);
        }

        /// <summary>
        /// try to get object from cache first if not there fall back to file. set up cache mock return null and the fileService to return a value.
        /// </summary>
        [Test]
        public async Task Should_SuccessGetFromFile()
        {
            _cacheSettingsMock.Setup(x => x.IsEnabled).Returns(true);
            _cacheSettingsFactoryMock.Setup(x => x.GetCacheSettings(CacheTypes.FileSystem, It.IsAny<string>())).Returns(_cacheSettingsMock.Object);
            _cacheManagerMock.Setup(x => x.Get<TestUser>(It.IsAny<string>())).Returns(Task.FromResult<TestUser>(null));
            _fileServiceMock.Setup(x => x.ReadXml<TestUser>(It.IsAny<string>())).Returns(Task.FromResult(new TestUser()));

            var fileManager = new FileManager(_loggerMock.Object, _cacheManagerMock.Object, _fileServiceMock.Object, _cacheSettingsFactoryMock.Object);
            var result = await fileManager.ReadXml<TestUser>("");
            Assert.NotNull(result);
        }


        /// <summary>
        /// try to get object from cache first if not there fall back to file. set up cache mock return null and the fileService to return a value.
        /// </summary>
        [Test]
        public async Task Should_SuccessCacheDisabled()
        {
            _cacheSettingsMock.Setup(x => x.IsEnabled).Returns(false);
            _cacheSettingsFactoryMock.Setup(x => x.GetCacheSettings(CacheTypes.FileSystem, It.IsAny<string>())).Returns(_cacheSettingsMock.Object);
            _cacheManagerMock.Setup(x => x.Get<TestUser>(It.IsAny<string>())).Returns(Task.FromResult<TestUser>(null));
            _fileServiceMock.Setup(x => x.ReadXml<TestUser>(It.IsAny<string>())).Returns(Task.FromResult(new TestUser()));

            var fileManager = new FileManager(_loggerMock.Object, _cacheManagerMock.Object, _fileServiceMock.Object, _cacheSettingsFactoryMock.Object);
            var result = await fileManager.ReadXml<TestUser>("");
            Assert.NotNull(result);
        }

        /// <summary>
        /// try to get object from cache first if not there then fall back to file. 
        /// log cacheManager exceptions 
        /// </summary>
        /// [Test]
        public async Task Should_LogCacheExceptionAndReadFile()
        {
            _loggerMock.Setup(x => x.LogError(It.IsAny<string>())).Verifiable();
            _cacheSettingsMock.Setup(x => x.IsEnabled).Returns(true);
            _cacheSettingsFactoryMock.Setup(x => x.GetCacheSettings(CacheTypes.FileSystem, "DummyFile")).Returns(_cacheSettingsMock.Object);
            _cacheManagerMock.Setup(x => x.Get<TestUser>(It.IsAny<string>())).Throws(new Exception("CacheManager Error."));
            _fileServiceMock.Setup(x => x.ReadXml<TestUser>(It.IsAny<string>())).Returns(Task.FromResult(new TestUser()));

            var fileManager = new FileManager(_loggerMock.Object, _cacheManagerMock.Object, _fileServiceMock.Object, _cacheSettingsFactoryMock.Object);
            var result = await fileManager.ReadXml<TestUser>("");
            Assert.NotNull(result);
        }
    }

    public class TestUser
    {
        public string FirstName => "firstname";
        public string LastName => "lastname";
    }

}
