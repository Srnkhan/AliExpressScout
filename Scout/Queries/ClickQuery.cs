using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Queries
{
    public class ClickQuery : CommonQuery
    {
        public string Selector { get; set; }
        public string WaitForSelector { get; set; }
    }
}
