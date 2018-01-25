using System;

namespace Lightrail.Params
{
    public interface IPaginationParams
    {
        int? Limit { get; set; }
        int? Offset { get; set; }
    }
}
