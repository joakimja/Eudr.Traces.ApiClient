using Eudr.Traces.Integrations.Authentications;
using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.Extensions;
using Eudr.Traces.Integrations.ServiceAgents.Interfaces;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;

using System.ServiceModel;

namespace Eudr.Traces.Integrations.ServiceAgents
{
    public class EUDRRetrievalServiceAgent : IEUDRRetrievalServiceAgent
    {
        private readonly EudrSettings _settings;

        private readonly string _webserviceUrl;

        public EUDRRetrievalServiceAgent(EudrSettings settings)
        {
            _settings = settings;
            _webserviceUrl = settings.ApiUrl + "/tracesnt/ws/EUDRRetrievalServiceV1";
            _settings.Validate();
        }

        public async Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<string> ddsIdentifiers)
        {
            EUDRRetrievalPortClient client = new EUDRRetrievalPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrl);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.getDdsInfoAsync(_settings.WebServiceClientId, ddsIdentifiers.ToArray());

            IEnumerable<StatementInfoType> statements=new List<StatementInfoType>();
            if (response.GetStatementInfoResponse1!=null)
            {
                statements = response.GetStatementInfoResponse1;
            }
            

          
            return statements;
        }
    }
}