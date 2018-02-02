using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class TransactionBreakdown
    {
        public long Value { get; set;}
        public long ValueAvailableAfterTransaction { get; set;}
        public string ValueStoreId { get; set;}
    }
}
