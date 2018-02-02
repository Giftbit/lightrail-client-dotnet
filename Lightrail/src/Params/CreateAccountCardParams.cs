using Lightrail.Model;
using System;
using System.Collections.Generic;

namespace Lightrail.Params
{
    public class CreateAccountCardParams : IUserSuppliedIdRequired
    {
        public string UserSuppliedId { get; set; }
        public string Currency { get; set; }
        public long InitialValue { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public string ContactId { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? Inactive { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public string ProgramId { get; set; }

        public static implicit operator CreateCardParams(CreateAccountCardParams c)
        {
            return new CreateCardParams
            {
                UserSuppliedId = c.UserSuppliedId,
                Currency = c.Currency,
                InitialValue = c.InitialValue,
                Categories = c.Categories,
                ContactId = c.ContactId,
                Expires = c.Expires,
                StartDate = c.StartDate,
                Inactive = c.Inactive,
                Metadata = c.Metadata,
                ProgramId = c.ProgramId,
                CardType = CardType.ACCOUNT_CARD
            };
        }
    }
}
