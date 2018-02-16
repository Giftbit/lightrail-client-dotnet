using System;

namespace Lightrail.Model
{
    public class Pagination
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int MaxLimit { get; set; }
        public int TotalCount { get; set; }
    }
}
