using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class CardDetails
    {
        public string Currency { get; set;}
        public CardType CardType { get; set; }
        public DateTime? AsAtDate { get; set; }
        public string CardId { get; set; }
        public IList<ValueStoreDetails> ValueStores { get; set; }
    }

    public class ValueStoreDetails
    {
        public ValueStoreType ValueStoreType { get; set; }
        public Int64 Value { get; set; }
        public BalanceState State { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? StartDate { get; set; }
        public string ProgramId { get; set; }
        public string ValueStoreId { get; set; }
        public IList<string> Restrictions { get; set; }
    }

    public enum BalanceState
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "INACTIVE")]
        INACTIVE,

        [EnumMember(Value = "FROZEN")]
        FROZEN,

        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "EXPIRED")]
        EXPIRED,

        [EnumMember(Value = "NOT_STARTED")]
        NOT_STARTED
    }
}
