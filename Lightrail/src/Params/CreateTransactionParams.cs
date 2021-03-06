using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class CreateTransactionParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public long Value { get; set; }
        public string Currency { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public bool? Pending { get; set; }
    }
}
