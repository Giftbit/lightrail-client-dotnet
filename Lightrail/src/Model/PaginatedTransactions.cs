using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedTransactions : IPaginated
    {
        public IList<Transaction> Transactions { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int MaxLimit { get; set; }
        public int TotalCount { get; set; }
    }
}
