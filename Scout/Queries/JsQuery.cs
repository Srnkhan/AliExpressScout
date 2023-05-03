using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Queries
{
    public class JsQuery : CommonQuery
    {
        public string Query { get; set; }
        public string WaitForSelector { get; set; }
    }
}
