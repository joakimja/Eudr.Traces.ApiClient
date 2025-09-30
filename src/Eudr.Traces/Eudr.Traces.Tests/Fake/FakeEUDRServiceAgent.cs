using Eudr.Traces.Integrations;
using Eudr.Traces.Integrations.Entities;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Tests.Fake
{
    public class FakeEUDRServiceAgent : IEUDRServiceAgent
    {
        private List<StatementInfoType> _statementInfoTypes;
        public FakeEUDRServiceAgent(List<StatementInfoType> statementInfoTypes)
        {
                _statementInfoTypes=statementInfoTypes;
        }
        public Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<Guid> ddsIdentifiers)
        {
            return Task.FromResult(_statementInfoTypes.Where(c => ddsIdentifiers.Select(g => g.ToString()).Contains(c.identifier)));
           
        }

      
        public Task<GetStatementByIdentifiersResponse> GetStatementByIdentifiersAsync(string reference, string verification)
        {
            throw new NotImplementedException();
        }

        public Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SubmitDdsAsync(DDSWithGps request)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SubmitDdsAsync(DDSWithReference request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateDDSAsync(string reference, string verification)
        {
            throw new NotImplementedException();
        }
    }
}
