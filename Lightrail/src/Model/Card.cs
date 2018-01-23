using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightrail.Model
{
    public class Card
    {
        public string CardId { get; set;}
        public string UserSuppliedId { get; set;}
        public CardType CardType { get; set;}
        public string Currency { get; set;}
        public string ContactId { get; set;}
        public DateTime DateCreated { get; set;}
        public List<CardCategory> Categories { get; set; }
    }

    public class CardCategory
    {
        public string CategoryId { get; set;}
        public string Key { get; set;}
        public string Value { get; set;}
    }

    public enum CardType
    {
        [EnumMember(Value = "GIFT_CARD")]
        GIFT_CARD,

        [EnumMember(Value = "ACCOUNT_CARD")]
        ACCOUNT_CARD
    }
}
