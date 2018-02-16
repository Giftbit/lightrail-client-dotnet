using Lightrail.Model;
using Lightrail.Params;
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightrail
{
    public class Programs
    {
        private LightrailClient _lightrail;

        internal Programs(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        public async Task<Program> CreateProgram(CreateProgramParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/programs")
                .AddBody(parms)
                .Execute<Dictionary<string, Program>>();
            response.EnsureSuccess();
            return response.Body["program"];
        }

        public async Task<PaginatedPrograms> GetPrograms(GetProgramsParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var response = await _lightrail.Request("GET", "v1/programs")
                .AddQueryParameters(parms)
                .Execute<PaginatedPrograms>();
            response.EnsureSuccess();
            return response.Body;
        }

        public async Task<Program> GetProgramById(string programId)
        {
            if (programId == null)
            {
                throw new ArgumentNullException(nameof(programId));
            }

            var response = await _lightrail.Request("GET", "v1/programs/{programId}")
                .SetPathParameter("programId", programId)
                .Execute<Dictionary<string, Program>>();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccess();
            return response.Body["program"];
        }
    }
}
