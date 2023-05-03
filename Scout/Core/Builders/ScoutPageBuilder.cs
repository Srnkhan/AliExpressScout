using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Core.Builders
{
    public class ScoutPageBuilder<T> : ScoutBuilder where T : ScoutPageBuilder<T>
    {
        /// <summary>
        /// Show Automated Explorer
        /// </summary>
        /// <param name="showHeader"></param>
        /// <returns></returns>
        public T Header(bool showHeader)
        {
            SCConfiguration.PageConfiguration.HeaderLess = !showHeader;
            return (T)this;

        }


        /// <summary>
        /// Set Automated Explorer With
        /// </summary>
        /// <param name="with"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T With(int with)
        {
            if (with <= 0)
            {
                throw new Exception("With must greater than zero");
            }
            SCConfiguration.PageConfiguration.Width = with;
            return (T)this;
        }


        /// <summary>
        /// Set Start Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public T Url(string url)
        {
            Uri validatedUri;

            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new Exception("Invalid Url");
            }
            SCConfiguration.PageConfiguration.StartUrl = url;
            return (T)this;
        }

        public T Browser(string browser)
        {
            if (string.IsNullOrWhiteSpace(browser))
            {
                throw new Exception("Invalid browser");
            }
            SCConfiguration.PageConfiguration.Browser = browser;
            return (T)this;
        }

        /// <summary>
        /// Set Automated Explorer Height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Height(int height)
        {
            if (height <= 0)
            {
                throw new Exception("Height must greater than zero");
            }
            SCConfiguration.PageConfiguration.Height = height;
            return (T)this;
        }


    }
}
