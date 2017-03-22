namespace Bepos.Reporting.Engine.Models
{
    public class BuildQueryResult
    {
        public string SqlStatement { get; set; }
        public object[] SqlParameters { get; set; }
    }
}
