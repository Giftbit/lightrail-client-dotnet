using System;

namespace Lightrail.Params
{
    public interface IUserSuppliedIdRequired
    {
        string UserSuppliedId { get; set; }
    }

    public static class IUserSuppliedIdRequiredExtension
    {
        public static void EnsureUserSuppliedId(this IUserSuppliedIdRequired usir)
        {
            if (usir.UserSuppliedId == null)
            {
                throw new ArgumentException("UserSuppliedId is required");
            }
        }
    }
}
