using Lightrail.Model;
using System;

namespace Lightrail.Params
{
    public class GetContactsParams : IPaginationParams
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserSuppliedId { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
