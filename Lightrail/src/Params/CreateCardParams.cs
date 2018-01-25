using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class CreateCardParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public CardType CardType { get; set; }
        public string Currency { get; set; }
        public Int64 InitialValue { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public string ContactId { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? Inactive { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public string ProgramId { get; set; }
    }
}
