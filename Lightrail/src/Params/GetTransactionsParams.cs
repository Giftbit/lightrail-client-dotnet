using Lightrail.Model;
using System;

namespace Lightrail.Params
{
    public class GetTransactionsParams : IPaginationParams
    {
        public string UserSuppliedId { get; set; }
        public TransactionType? TransactionType { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
