using Lightrail;
using Lightrail.Net;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task TestLogger()
        {
            var logger = new StringBuilderLogger();
            var lightrail = new LightrailClient()
            {
                ApiKey = "asdasdadsadsda",
                Logger = logger   
            };
            await lightrail.Request("GET", "/v1/ping").Execute<object>();
            var logs = logger.SB.ToString();

            Assert.IsTrue(logs.StartsWith("GET "), $"has logs -> {logs}");
        }

        /// Stores all logs in a StringBuilder for later inspection.
        private class StringBuilderLogger : ILogger
        {
            public StringBuilder SB = new StringBuilder();
            public IDisposable BeginScope<TState>(TState state) => null;
            public bool IsEnabled(LogLevel logLevel) => true;
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => SB.Append(formatter(state, exception));
        }
    }
}
