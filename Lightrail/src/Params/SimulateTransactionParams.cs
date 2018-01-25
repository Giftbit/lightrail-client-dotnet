using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class SimulateTransactionParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public Int64 Value { get; set; }
        public string Currency { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public bool? Nsf { get; set; }
    }
}
