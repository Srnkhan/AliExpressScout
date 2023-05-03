using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Queries
{
    public class ScrollQuery : CommonQuery
    {
        public int ScrollHeight { get; set; } = 0;
    }
}
