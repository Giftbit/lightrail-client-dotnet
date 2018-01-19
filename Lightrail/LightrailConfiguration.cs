namespace Lightrail
{

    public static class LightrailConfiguration
    {
        public static string ApiKey { get; set; }

        public static string SharedSecret { get; set; }

        public static string RestRoot { get; set; } = "https://api.lightrail.com/";

        // TODO figure out correct typing here
        //public static object AdditionalHeaders { get; set; }

    }
}
