using Lightrail.Model;
using Lightrail.Params;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lightrail
{
    public class Contacts
    {
        private LightrailClient _lightrail;

        internal Contacts(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        public async Task<Contact> CreateContact(CreateContactParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/contacts")
                .AddBody(parms)
                .Execute<Dictionary<string, Contact>>();
            response.EnsureSuccess();
            return response.Body["contact"];
        }

        public async Task<Contact> GetContact(ContactIdentifier ci)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            if (ci.ContactId != null)
            {
                return await GetContactById(ci.ContactId);
            }
            else if (ci.UserSuppliedId != null)
            {
                return await GetContactByUserSuppliedId(ci.UserSuppliedId);
            }
            else if (ci.ShopperId != null)
            {
                return await GetContactByUserSuppliedId(ci.ShopperId);
            }
            throw new ArgumentException("one of ContactId, UserSuppliedId or ShopperId must be set");
        }

        public async Task<Contact> GetContactById(string contactId)
        {
            if (contactId == null)
            {
                throw new ArgumentNullException(nameof(contactId));
            }

            var response = await _lightrail.Request("GET", "v1/contacts/{contactId}")
                .SetPathParameter("contactId", contactId)
                .Execute<Dictionary<string, Contact>>();
            response.EnsureSuccess();
            return response.Body["contact"];
        }

        public async Task<Contact> GetContactByUserSuppliedId(string userSuppliedId)
        {
            if (userSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(userSuppliedId));
            }

            var resp = await GetContacts(new GetContactsParams { UserSuppliedId = userSuppliedId });
            if (resp.Contacts.Count > 0)
            {
                return resp.Contacts[0];
            }
            return null;
        }

        public async Task<PaginatedContacts> GetContacts(GetContactsParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var response = await _lightrail.Request("GET", "v1/contacts")
                .AddQueryParameters(parms)
                .Execute<PaginatedContacts>();
            response.EnsureSuccess();
            return response.Body;
        }

        public async Task<Contact> UpdateContact(string contactId, UpdateContactParams parms)
        {
            if (contactId == null)
            {
                throw new ArgumentNullException(nameof(contactId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var response = await _lightrail.Request("PATCH", "v1/contacts/{contactId}")
                .SetPathParameter("contactId", contactId)
                .AddBody(parms)
                .Execute<Dictionary<string, Contact>>();
            response.EnsureSuccess();
            return response.Body["contact"];
        }

        public async Task<Contact> UpdateContact(ContactIdentifier ci, UpdateContactParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            if (ci.ContactId != null)
            {
                return await UpdateContact(ci.ContactId, parms);
            }
            else
            {
                var contact = await GetContact(ci);
                return await UpdateContact(contact.ContactId, parms);
            }
        }
    }
}
