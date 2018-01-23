using System;
using System.Collections.Generic;

namespace Lightrail
{

    public static class LightrailConfiguration
    {
        public static string ApiKey { get; set; }

        public static string SharedSecret { get; set; }

        public static string RestRoot { get; set; } = "https://api.lightrail.com";

        public static List<KeyValuePair<string, string>> AdditionalHeaders { get; set; }

    }
}
