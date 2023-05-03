using System;

namespace Contracts
{
    public record SecondLayer
    {
        public Guid CategoryId { get; init; }
        public string CategoryUrl { get; init; }
        public string CategoryName { get; init; }
    }
}