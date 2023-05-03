using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Domain
{
    internal class CommandInformation
    {
        private string BeforeUrl;
        private string AfterUrl;
        public CommandInformation()
        {
            BeforeUrl = "";
            AfterUrl = "";
        }
        public CommandInformation(string beforeUrl , string afterUrl)
        {
            BeforeUrl = beforeUrl;
            AfterUrl = afterUrl;
        }
        public void SetBeforeUrl(string url) => this.BeforeUrl = url;
        public void SetAfterUrl(string url) => this.AfterUrl = url;

    }
}
