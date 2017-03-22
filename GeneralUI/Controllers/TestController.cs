using System.Threading.Tasks;
using Infrastructure.Interfaces.IManagers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TestUI.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileManager _fileManager;
        private readonly ILogger<TestController> _logger;
        public TestController(IHostingEnvironment hostingEnvironment, IFileManager fileManager, ILogger<TestController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileManager = fileManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<EndPoints> ReadXml([FromQuery]string fileName)
        {
            var fileSysName = _hostingEnvironment.ContentRootPath + fileName;

            return await _fileManager.ReadXml<EndPoints>(@"data\Endpoints.Production.xml");
        }

    }
}
