using System;

namespace Lightrail.Model
{
    public interface IPaginated
    {
        int Count { get; set; }
        int Offset { get; set; }
        int Limit { get; set; }
        int MaxLimit { get; set; }
        int TotalCount { get; set; }
    }
}
