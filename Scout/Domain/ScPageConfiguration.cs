using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Domain
{
    internal class ScPageConfiguration
    {
        public string StartUrl { get; set; }
        public bool HeaderLess { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>
        /// BrowserFetcher Options
        /// </summary>
        public string Browser { get; set; }
        public ScPageConfiguration()
        {
            StartUrl = string.Empty;
            Browser = BrowserFetcher.DefaultChromiumRevision;
        }
    }
}
