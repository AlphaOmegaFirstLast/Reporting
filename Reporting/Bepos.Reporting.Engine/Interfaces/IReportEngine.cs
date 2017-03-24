using System.Threading.Tasks;
using Bepos.Reporting.Engine.Models;

namespace Bepos.Reporting.Engine.Interfaces
{
    public interface IReportEngine
    {
        BuildQueryResult BuildQuery(ReportCriteria criteria, bool onlyRecordsCount = false);
        Task<T> ExecuteQuery<T>(string sql, params object[] parameters);
        Task<PaginationInfo> GetPaginationInfo(ReportCriteria criteria);
    }
}