using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;

namespace Eudr.Traces.Integrations
{
    public interface IEUDRServiceAgent
    {
        Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<string> ddsIdentifiers);
        Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request);
    }
}