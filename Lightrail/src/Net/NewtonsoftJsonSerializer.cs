using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Lightrail.Net
{
    internal class NewtonSoftJsonSerializer : RestSharp.Serializers.ISerializer
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new [] {new StringEnumConverter()}
        };

		public string DateFormat { get; set; }
		public string RootElement { get; set; }
		public string Namespace { get; set; }
        public string ContentType { get; set; } = "application/json";

        public string Serialize(object obj) {
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
