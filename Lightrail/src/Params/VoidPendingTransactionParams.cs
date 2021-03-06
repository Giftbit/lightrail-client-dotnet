using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class VoidPendingTransactionParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
    }
}
