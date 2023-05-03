using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Queries
{
    public enum QueryType
    {
        CategoryUrlList = 1,
        CategoryNameList = 2,
        ProductList = 3,
        ProductListNextPage = 4,
        ProductLastPage = 5,
    }
}
