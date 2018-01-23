using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class ValueStore
    {
        public string CardId { get; set; }
        public string ValueStoreId { get; set; }
        public ValueStoreType ValueStoreType { get; set; }
        public string Currency { get; set; }
        public DateTime DateCreated { get; set; }
        public string ProgramId { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public enum ValueStoreType
    {
        [EnumMember(Value = "PRINCIPAL")]
        PRINCIPAL,
        
        [EnumMember(Value = "ATTACHED")]
        ATTACHED
    }
}
