using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class Program
    {
        public string ProgramId { get; set; }
        public string UserSuppliedId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Currency { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? ProgramExpiresDate { get; set; }
        public DateTime? ProgramStartDate { get; set; }
        public long? CodeActivePeriodInDays { get; set; }
        public long? CodeValueMin { get; set; }
        public long? CodeValueMax { get; set; }
        public IList<long> FixedCodeValues { get; set; }
        public CodeEngine CodeEngine { get; set; }
        public CodeConfig CodeConfig { get; set; }
        public ValueStoreType ValueStoreType { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public string TimeZone { get; set; }
        public CardType CardType { get; set; }
    }

    public enum CodeEngine
    {
        [EnumMember(Value = "SIMPLE_STORED_VALUE")]
        SIMPLE_STORED_VALUE
    }

    public enum CodeConfig
    {
        [EnumMember(Value = "DEFAULT")]
        DEFAULT
    }
}
