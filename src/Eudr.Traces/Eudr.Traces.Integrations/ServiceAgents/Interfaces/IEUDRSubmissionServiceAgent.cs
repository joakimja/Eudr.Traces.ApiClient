using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;

namespace Eudr.Traces.Integrations.ServiceAgents.Interfaces
{
    public interface IEUDRSubmissionServiceAgent
    {
        Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request);
    }
}