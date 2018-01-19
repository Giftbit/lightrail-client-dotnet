using System;
using System.IdentityModel.Tokens;
using System.Text;

namespace Lightrail
{

    public static class ShopperTokenFactory
    {
        
        public static string GenerateShopperToken(string shopperId, int validityInSeconds = 43200)
        {
            if (LightrailConfiguration.ApiKey == null)
            {
                // TODO throw a better Exception type
                throw new Exception("LightrailConfiguration.ApiKey is not set.");
            }
            if (LightrailConfiguration.SharedSecret == null)
            {
                // TODO throw a better Exception type
                throw new Exception("LightrailConfiguration.SharedSecret is not set.");
            }
            if (shopperId == null)
            {
                throw new ArgumentNullException(nameof(shopperId));
            }
            if (validityInSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(validityInSeconds), "must be > 0");
            }

            

            return "TODO";
        }
    }
}
