using System;

namespace Lightrail.Model
{
    public interface IPaginated
    {
        Pagination Pagination { get; set; }
    }
}
