using PuppeteerSharp;
using Scout.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Core.Builders
{
    public abstract class ScoutBuilder
    {
        internal SCConfiguration SCConfiguration;


        internal IPage Page;

        public ScoutBuilder()
        {
            SCConfiguration = new SCConfiguration();
        }


        /// <summary>
        /// Prepare Page
        /// </summary>
        /// <returns></returns>
        public async Task<IPage> PrepareAsync()
        {
            var explorerConfig = SCConfiguration.PageConfiguration;
            using var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync(explorerConfig.Browser);

            var url = explorerConfig.StartUrl;

            var launchOptions = new LaunchOptions()
            {
                Headless = explorerConfig.HeaderLess,

                DefaultViewport = new ViewPortOptions
                {
                    Width = explorerConfig.Width,
                    Height = explorerConfig.Height
                }
            };

            var browser = await Puppeteer.LaunchAsync(launchOptions);
            Page = await browser.NewPageAsync();
            await Page.GoToAsync(url , 60000);

            return Page;
        }

        public override string? ToString()
        {
            return base.ToString();
        }

    }
}
