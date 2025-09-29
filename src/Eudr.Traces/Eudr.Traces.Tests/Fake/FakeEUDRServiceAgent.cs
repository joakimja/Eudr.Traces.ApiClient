using Eudr.Traces.Integrations;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Tests.Fake
{
    internal class FakeEUDRServiceAgent : IEUDRServiceAgent
    {
        public Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<string> ddsIdentifiers)
        {
            throw new NotImplementedException();
        }

        public Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request)
        {
            throw new NotImplementedException();
        }
    }
}
