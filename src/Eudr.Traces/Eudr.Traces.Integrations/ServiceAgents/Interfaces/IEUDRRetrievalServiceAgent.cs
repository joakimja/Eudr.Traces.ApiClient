using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;

namespace Eudr.Traces.Integrations.ServiceAgents.Interfaces
{
    public interface IEUDRRetrievalServiceAgent
    {
        Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<string> ddsIdentifiers);
    }
}