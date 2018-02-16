using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedContacts : IPaginated
    {
        public IList<Contact> Contacts { get; set; }
        public Pagination Pagination { get; set; }
    }
}
