namespace Bepos.Reporting.Engine.Models
{
    public class ReportField
    {
        public string Caption { get; set; }
        public string Field{ get; set; }
        public string LookupTable { get; set; }
        public string LookupField { get; set; }
        public string LookupFieldValue { get; set; }
        public string SummaryOp{ get; set; }
        public string SummaryCaption{ get; set; }
    }
}