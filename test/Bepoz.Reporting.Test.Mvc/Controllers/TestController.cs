using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Bepos.Reporting.Engine.Interfaces;
using Bepos.Reporting.Engine.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.v3;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bepoz.Reporting.Test.Mvc.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IReportEngine _reportEngine;

        public TestController(  IReportEngine reportEngine)
        {
            _reportEngine = reportEngine;
        }

        // POST api/values
        [HttpPost("GetCriteria")]
        public string GetCriteria([FromBody] ReportCriteria criteria)
        {
            var result = criteria.ToJson();
            return result;
        }

        // POST api/values
        [HttpPost("GetSql")]
        public string GetSql([FromBody] ReportCriteria criteria)
        {
            var result = _reportEngine.BuildQuery(criteria);
            return result.SqlStatement;
        }

        [HttpPost("GetReports")]
        public async Task<ApiResponse<string>> GetReports([FromBody] ReportCriteria criteria)
        {
            var apiResponse = new ApiResponse<string>() {ReportCriteria = criteria};
            try
            {
                var resultCriteria = _reportEngine.BuildQuery(criteria);
                var resultQuery = await _reportEngine.ExecuteQuery<string>(resultCriteria.SqlStatement, resultCriteria.SqlParameters);

                var doc = new XmlDocument();
                doc.LoadXml(resultQuery);
                var jsonResult = JsonConvert.SerializeXmlNode(doc);

                //remove @ from output json
                var jsonText = Regex.Replace(jsonResult, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);

                apiResponse.Data = jsonText;
                apiResponse.Pagination = await _reportEngine.GetPaginationInfo(criteria);
            }
            catch (Exception e)
            {
                apiResponse.Status.SetError(1,"Error GetReports: ",e);
            }
            return apiResponse;

        }

        //    [HttpPost("GetReportsNotParamaterised")]
        //    public async Task<string> GetReportsNotParamaterised([FromBody] ReportCriteria criteria)
        //    {
        //        var resultCriteria = _reportContext.GetCriteriaQuery(criteria);
        //        var resultReport = await _reportContext.SqlQuery<string>(resultCriteria.Item1);

        //        var doc = new XmlDocument();
        //        doc.LoadXml(resultReport[0]);
        //        var jsonResult = JsonConvert.SerializeXmlNode(doc);
        //        //remove @ from output json
        //        var jsonText = Regex.Replace(jsonResult, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
        //        return jsonText;
        //    }

        //    [HttpPost("GetReportsInXml")]
        //    public async Task<string> GetReportsInXml([FromBody] ReportCriteria criteria)
        //    {
        //        var result = _reportContext.GetCriteriaQuery(criteria);
        //        var resultReport = await _reportContext.SqlQuerySerialized(result.Item1, result.Item2);

        //        return resultReport;
        //    }

        //    // POST api/values
        //    [HttpPost("GetAll")]
        //    public async Task<List<Employee>> GetAll([FromBody] object criteria)
        //    {
        //        var result = await _reportContext.SqlQuery<Employee>("select * from Employees");
        //        return result;
        //    }


        //    [HttpGet("GetEmp1")]
        //    public async Task<List<string>> GetEmp1()
        //    {
        //        var result = await _reportContext.SqlQuery<string>("select id,name from Employees where id>@myid FOR XML RAW ('result'), ROOT ('results');" , 2);
        //        return result;
        //    }

        //    [HttpGet("GetEmp2")]
        //    public async Task<List<string>> GetEmp2()
        //    {
        //        var result = await _reportContext.SqlQuery<string>("select * from Employees FOR XML RAW ('Employee'), ROOT ('Root');");
        //        return result;
        //    }
    }
}

