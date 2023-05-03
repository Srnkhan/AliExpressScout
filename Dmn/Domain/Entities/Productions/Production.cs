using Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Productions
{
    public class Production : Base
    {
        public string Url { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public string? Price { get; set; }
        public string? DiscoutLessPrice { get; set; }
        public int? Star { get; set; }
        public int? Review { get; set; }
        public Currency Currency { get; set; }
        private Production()
        {

        }
        public Production(string url, string name, string detail, Guid categoryId, string? price, string? discoutLessPrice, int? star, int? review)
        {
            Url = url;
            Name = name;
            Description = detail;
            CategoryId = categoryId;
            Price = price;
            DiscoutLessPrice = discoutLessPrice;
            Star = star;
            Review = review;
        }
    }
}
