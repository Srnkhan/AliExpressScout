using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Queries
{
    public class Query : Base
    {
        public string QueryText { get; private set; }
        public string Description { get; private set; }
        public QueryType Type { get; private set; }
        public bool IsActive { get; private set; }

        public Query(Guid id, string queryText, string description, QueryType type, bool isActive)
        {
            Id = id;
            QueryText = queryText;
            Description = description;
            Type = type;
            IsActive = isActive;
        }
    }
}
