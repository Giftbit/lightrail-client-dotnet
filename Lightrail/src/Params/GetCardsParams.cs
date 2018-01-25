using Lightrail.Model;
using System;

namespace Lightrail.Params
{
    public class GetCardsParams : IPaginationParams
    {
        public string ContactId { get; set; }
        public CardType? CardType { get; set; }
        public string Currency { get; set; }
        public string UserSuppliedId { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
