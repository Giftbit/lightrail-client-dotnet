using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class CreateAccountCardParams : IUserSuppliedIdRequired
    {
        /// <summary>
        /// The UserSuppliedId for the card.
        /// </summary>
        public string UserSuppliedId { get; set; }
        
        public string Currency { get; set; }
        public long InitialValue { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? Inactive { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public string ProgramId { get; set; }
    }
}
