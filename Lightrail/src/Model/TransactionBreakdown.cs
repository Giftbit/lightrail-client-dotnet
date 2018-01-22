using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class TransactionBreakdown
    {
        public Int64 Value { get; set;}
        public Int64 ValueAvailableAfterTransaction { get; set;}
        public string ValueStoreId { get; set;}
    }
}
