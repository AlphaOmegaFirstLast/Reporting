using System.Collections.Generic;

namespace Bepos.Reporting.Engine.Models
{
    public class ReportCriteria
    {
        private int _currentPage = 0 ;
        private int _recordsPerPage = 10;
        public string Title{ get; set; }
        public string MainTable { get; set; }
        public string MainTablePrimaryKey { get; set; }

        public List<ReportField> DisplayFields{ get; set; }
        public List<ReportField> GroupBy { get; set; }
        public List<ReportField> OrderBy { get; set; }
        public List<ReportFilter> ValueFilters{ get; set; }
        public List<ReportFilter> RangeFilters { get; set; }


        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set { _recordsPerPage = value; }
        }
    }
}
