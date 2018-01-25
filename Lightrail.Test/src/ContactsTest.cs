using Lightrail;
using Lightrail.Model;
using Lightrail.Net.Exceptions;
using Lightrail.Params;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lightrail.Test
{
    [TestClass]
    public class ContactsTest
    {
        private LightrailClient _lightrail;

        [TestInitialize]
        public void Before()
        {
            DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env"));
            _lightrail = new LightrailClient
            {
                ApiKey = Environment.GetEnvironmentVariable("LIGHTRAIL_API_KEY")
            };
        }

        [TestMethod]
        public async Task TestCreateAndGetAndUpdateContact()
        {
            var userSuppliedId = Guid.NewGuid().ToString();

            var contact = await _lightrail.Contacts.CreateContact(new CreateContactParams
            {
                UserSuppliedId = userSuppliedId,
                Email = "jammy@wammy.com",
                FirstName = "Jamal",
                LastName = "Marais"
            });
            Assert.IsNotNull(contact);
            Assert.IsNotNull(contact.ContactId);
            Assert.AreEqual(userSuppliedId, contact.UserSuppliedId);
            Assert.AreEqual("jammy@wammy.com", contact.Email);
            Assert.AreEqual("Jamal", contact.FirstName);
            Assert.AreEqual("Marais", contact.LastName);

            var contactById = await _lightrail.Contacts.GetContactById(contact.ContactId);
            Assert.IsNotNull(contactById);
            Assert.AreEqual(contact.ContactId, contactById.ContactId);
            Assert.AreEqual(contact.UserSuppliedId, contactById.UserSuppliedId);
            Assert.AreEqual(contact.Email, contactById.Email);
            Assert.AreEqual(contact.FirstName, contactById.FirstName);
            Assert.AreEqual(contact.LastName, contactById.LastName);

            // Jamal is a progressive kind of guy and took his wife's last name.
            var updatedContact = await _lightrail.Contacts.UpdateContact(contactById.ContactId, new UpdateContactParams { LastName = "Coetzee" });
            Assert.IsNotNull(updatedContact);
            Assert.AreEqual(contact.ContactId, updatedContact.ContactId);
            Assert.AreEqual(contact.UserSuppliedId, updatedContact.UserSuppliedId);
            Assert.AreEqual(contact.Email, updatedContact.Email);
            Assert.AreEqual(contact.FirstName, updatedContact.FirstName);
            Assert.AreEqual("Coetzee", updatedContact.LastName);

            var contactByUserSuppliedId = await _lightrail.Contacts.GetContactByUserSuppliedId(userSuppliedId);
            Assert.IsNotNull(contactByUserSuppliedId);
            Assert.AreEqual(updatedContact.ContactId, contactByUserSuppliedId.ContactId);
            Assert.AreEqual(updatedContact.UserSuppliedId, contactByUserSuppliedId.UserSuppliedId);
            Assert.AreEqual(updatedContact.Email, contactByUserSuppliedId.Email);
            Assert.AreEqual(updatedContact.FirstName, contactByUserSuppliedId.FirstName);
            Assert.AreEqual(updatedContact.LastName, contactByUserSuppliedId.LastName);
        }

        [TestMethod]
        public async Task TestGetContactNotFound()
        {
            var contact = await _lightrail.Contacts.GetContactById(Guid.NewGuid().ToString());
            Assert.IsNull(contact);
        }
    }
}
