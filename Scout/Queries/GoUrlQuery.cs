using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Queries
{
    public class GoUrlQuery:CommonQuery
    {
        public string Url { get; set; }
        public string WaitForSelector { get; set; }
    }
}
