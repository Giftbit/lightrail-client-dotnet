using Lightrail;
using Lightrail.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lightrail.Test.Net
{
    [TestClass]
    public class LightrailRequestTest
    {
        [TestMethod]
        public void TestAddQueryParametersObject()
        {
            var parms = new {
                zero = 0,
                one = 1,
                bfalse = false,
                btrue = true,
                str = "immastring"
            };

            var lightrail = new LightrailClient();
            var req = lightrail.Request("GET", "/foo")
                .AddQueryParameters(parms);

            Assert.AreEqual("?zero=0&one=1&bfalse=False&btrue=True&str=immastring", req.RequestUri.Query.ToString());
        }
    }
}
