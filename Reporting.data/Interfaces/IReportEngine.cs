using System;
using System.Threading.Tasks;
using Reporting.data.Models;

namespace Reporting.data.Interfaces
{
    public interface IReportEngine
    {
        BuildQueryResult BuildQuery(ReportCriteria criteria, bool onlyRecordsCount = false);
        Task<T> ExecuteQuery<T>(string sql, params object[] parameters);
        Task<PaginationInfo> GetPaginationInfo(ReportCriteria criteria);
    }
}