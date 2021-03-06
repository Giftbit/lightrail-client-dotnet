using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedCards : IPaginated
    {
        public IList<Card> Cards { get; set; }
        public Pagination Pagination { get; set; }
    }
}
