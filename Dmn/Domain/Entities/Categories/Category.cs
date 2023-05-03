using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Categories
{
    public class Category : Base
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public int Order { get; private set; }
        public Category(string name, string url,int order)
        {
            Name = name;
            Url = url;
            Order = order;
        }
        private Category()
        {

        }
    }
}
