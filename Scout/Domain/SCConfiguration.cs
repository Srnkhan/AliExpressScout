using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Domain
{
    internal class SCConfiguration
    {
        public ScPageConfiguration PageConfiguration { get; set; }

        public SCConfiguration()
        {
            PageConfiguration = new ScPageConfiguration();
        }
    }
}
