using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class CreateProgramParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public long? CodeMinValue { get; set; }
        public long? CodeMaxValue { get; set; }
        public long[] FixedCodeValues { get; set; }
        public DateTime? ProgramStartDate { get; set; }
        public DateTime? ProgramExpiresDate { get; set; }
        public CardType? CardType { get; set; }
        public ValueStoreType? ValueStoreType { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
    }
}
