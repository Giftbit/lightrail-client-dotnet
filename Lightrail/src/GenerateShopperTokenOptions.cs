using System;
using System.Collections.Generic;

namespace Lightrail
{
    public class GenerateShopperTokenOptions
    {
        public int? ValidityInSeconds { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
    }
}
