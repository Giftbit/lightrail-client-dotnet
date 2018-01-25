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
            Assert.AreEqual(contact.UserSuppliedId, userSuppliedId);
            Assert.AreEqual(contact.Email, "jammy@wammy.com");
            Assert.AreEqual(contact.FirstName, "Jamal");
            Assert.AreEqual(contact.LastName, "Marais");

            var contactById = await _lightrail.Contacts.GetContactById(contact.ContactId);
            Assert.IsNotNull(contactById);
            Assert.AreEqual(contactById.ContactId, contact.ContactId);
            Assert.AreEqual(contactById.UserSuppliedId, contact.UserSuppliedId);
            Assert.AreEqual(contactById.Email, contact.Email);
            Assert.AreEqual(contactById.FirstName, contact.FirstName);
            Assert.AreEqual(contactById.LastName, contact.LastName);

            // Jamal is a progressive kind of guy and took his wife's last name.
            var updatedContact = await _lightrail.Contacts.UpdateContact(contactById.ContactId, new UpdateContactParams { LastName = "Coetzee" });
            Assert.IsNotNull(updatedContact);
            Assert.AreEqual(updatedContact.ContactId, contact.ContactId);
            Assert.AreEqual(updatedContact.UserSuppliedId, contact.UserSuppliedId);
            Assert.AreEqual(updatedContact.Email, contact.Email);
            Assert.AreEqual(updatedContact.FirstName, contact.FirstName);
            Assert.AreEqual(updatedContact.LastName, "Coetzee");

            var contactByUserSuppliedId = await _lightrail.Contacts.GetContactByUserSuppliedId(userSuppliedId);
            Assert.IsNotNull(contactByUserSuppliedId);
            Assert.AreEqual(contactByUserSuppliedId.ContactId, updatedContact.ContactId);
            Assert.AreEqual(contactByUserSuppliedId.UserSuppliedId, updatedContact.UserSuppliedId);
            Assert.AreEqual(contactByUserSuppliedId.Email, updatedContact.Email);
            Assert.AreEqual(contactByUserSuppliedId.FirstName, updatedContact.FirstName);
            Assert.AreEqual(contactByUserSuppliedId.LastName, updatedContact.LastName);
        }
    }
}
