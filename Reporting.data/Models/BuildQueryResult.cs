using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reporting.data.Models
{
    public class BuildQueryResult
    {
        public string SqlStatement { get; set; }
        public object[] SqlParameters { get; set; }
    }
}
