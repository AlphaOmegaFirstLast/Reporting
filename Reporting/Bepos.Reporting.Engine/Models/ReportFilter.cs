namespace Bepos.Reporting.Engine.Models
{
    public class ReportFilter
    {
        public string Caption{ get; set; }
        public string Field{ get; set; }
        public string Value{ get; set; }
        public string LookupTable { get; set; }
        public string LookupField{ get; set; }
        public string LookupFieldValue{ get; set; }
        public string FromValue{ get; set; }
        public string ToValue{ get; set; }
    }
}