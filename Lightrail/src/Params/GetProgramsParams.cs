using Lightrail.Model;
using System;

namespace Lightrail.Params
{
    public class GetProgramsParams : IPaginationParams
    {
        public string Currency { get; set; }
        public ValueStoreType? ValueStoreType { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
