using System;
using System.Collections.Generic;

namespace Lightrail.Model
{
    public class PaginatedPrograms : IPaginated
    {
        public IList<Program> Programs { get; set; }
        public Pagination Pagination { get; set; }
    }
}
