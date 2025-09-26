using Eudr.Traces.Integrations.Authentications;
using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.Extensions;
using Eudr.Traces.Integrations.ServiceAgents.Interfaces;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System.ServiceModel;

namespace Eudr.Traces.Integrations.ServiceAgents
{
    public class EUDRSubmissionServiceAgent : IEUDRSubmissionServiceAgent
    {
        private readonly EudrSettings _settings;

        private readonly string _webserviceUrl;

        public EUDRSubmissionServiceAgent(EudrSettings settings)
        {
            _settings = settings;
            _webserviceUrl = settings.ApiUrl + "/tracesnt/ws/EUDRSubmissionServiceV1";
            _settings.Validate();
        }

        public async Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request)
        {
            EUDRSubmissionPortClient client = new EUDRSubmissionPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrl);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.submitDdsAsync(_settings.WebServiceClientId, request);
           
            return response;
        }
    }
}