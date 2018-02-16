using Lightrail;
using Lightrail.Model;
using Lightrail.Net.Exceptions;
using Lightrail.Params;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lightrail.Test
{
    [TestClass]
    public class ProgramsTest
    {
        private LightrailClient _lightrail;

        [TestInitialize]
        public void Before()
        {
            DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env"));
            _lightrail = new LightrailClient
            {
                ApiKey = Environment.GetEnvironmentVariable("LIGHTRAIL_API_KEY"),
                Logger = new LoggerFactory().AddConsole().CreateLogger("Lightrail")
            };
        }

        [TestMethod]
        public async Task TestCreateAndGetProgram()
        {
            var userSuppliedId = Guid.NewGuid().ToString();

            var program = await _lightrail.Programs.CreateProgram(new CreateProgramParams
            {
                UserSuppliedId = userSuppliedId,
                Name = ".net programs unit test",
                Currency = "USD",
                CodeMinValue = 10,
                CodeMaxValue = 1000,
                ValueStoreType = ValueStoreType.PRINCIPAL,
                ProgramStartDate = new DateTime(0)
            });
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ProgramId);
            Assert.AreEqual(userSuppliedId, program.UserSuppliedId);
            Assert.AreEqual(ValueStoreType.PRINCIPAL, program.ValueStoreType);
            Assert.AreEqual("USD", program.Currency);

            var programById = await _lightrail.Programs.GetProgramById(program.ProgramId);
            Assert.IsNotNull(programById);
            Assert.AreEqual(program.ProgramId, programById.ProgramId);
            Assert.AreEqual(program.UserSuppliedId, programById.UserSuppliedId);
            Assert.AreEqual(program.CardType, programById.CardType);
            Assert.AreEqual(program.Currency, programById.Currency);

            var programsByCurrency = await _lightrail.Programs.GetPrograms(new GetProgramsParams
            {
                Currency = "USD"
            });
            Assert.IsNotNull(programsByCurrency);
            Assert.IsTrue(programsByCurrency.Pagination.Count > 0);
            Assert.IsTrue(programsByCurrency.Programs.Count > 0);
        }
    }
}
