using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedContacts : IPaginated
    {
        public IList<Contact> Contacts { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int MaxLimit { get; set; }
        public int TotalCount { get; set; }
    }
}
