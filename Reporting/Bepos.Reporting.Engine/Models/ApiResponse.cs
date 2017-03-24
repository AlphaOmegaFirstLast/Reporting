
namespace Bepos.Reporting.Engine.Models
{
    public class ApiResponse<T>
    {
        public ApiStatus Status { get; set; }
        public T Data { get; set; }
        public ReportCriteria ReportCriteria { get; set; }
        public PaginationInfo Pagination { get; set; }

        public ApiResponse()
        {
            Status = new ApiStatus();
            ReportCriteria = new ReportCriteria();
            Pagination = new PaginationInfo();
        }
    }
}
