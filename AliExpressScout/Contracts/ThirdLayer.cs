using System;

namespace Contracts
{
    public record ThirdLayer
    {
        public Guid CategoryId{ get; set; }
        public string CategoryUrl { get; init; }
        public string CategoryName { get; init; }
        public int Order { get; set; }
    }
}