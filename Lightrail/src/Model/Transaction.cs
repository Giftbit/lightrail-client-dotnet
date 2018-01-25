using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public Int64 Value { get; set; }
        public string UserSuppliedId { get; set; }
        public DateTime DateCreated { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionAccessMethod TransactionAccessMethod { get; set; }
        public string ParentTransactionId { get; set; }
        public string CardId { get; set; }
        public string Currency { get; set; }
        public List<TransactionBreakdown> TransactionBreakdown { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    public enum TransactionType
    {
        [EnumMember(Value = "DRAWDOWN")]
        DRAWDOWN,

        [EnumMember(Value = "FUND")]
        FUND,

        [EnumMember(Value = "INITIAL_VALUE")]
        INITIAL_VALUE,

        [EnumMember(Value = "CANCELLATION")]
        CANCELLATION,

        [EnumMember(Value = "INACTIVATE")]
        INACTIVATE,

        [EnumMember(Value = "ACTIVATE")]
        ACTIVATE,

        [EnumMember(Value = "FREEZE")]
        FREEZE,

        [EnumMember(Value = "UNFREEZE")]
        UNFREEZE,

        [EnumMember(Value = "PENDING_CREATE")]
        PENDING_CREATE,

        [EnumMember(Value = "PENDING_VOID")]
        PENDING_VOID,

        [EnumMember(Value = "PENDING_CAPTURE")]
        PENDING_CAPTURE,

        [EnumMember(Value = "DRAWDOWN_REFUND")]
        DRAWDOWN_REFUND
    }

    public enum TransactionAccessMethod
    {
        [EnumMember(Value = "CARDID")]
        CARDID,

        [EnumMember(Value = "RAWCODE")]
        RAWCODE
    }
}
