using System;
using System.Threading.Tasks;
using Reporting.data.Models;

namespace Reporting.data.Interfaces
{
    public interface IReportEngine
    {
        BuildQueryResult BuildQuery(ReportCriteria criteria);
        Task<string> ExecuteQuery(string sql, params object[] parameters);
    }
}