using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedTransactions : IPaginated
    {
        public IList<Transaction> Transactions { get; set; }
        public Pagination Pagination { get; set; }
    }
}
