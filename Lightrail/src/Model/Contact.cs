using System;

namespace Lightrail.Model
{
    public class Contact : ContactIdentifier
    {
        public string Email { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
