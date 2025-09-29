using Eudr.Traces.Integrations.Authentications;
using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System.ServiceModel;

namespace Eudr.Traces.Integrations
{
    public class EUDRServiceAgent : IEUDRServiceAgent
    {
        private readonly EudrSettings _settings;

        private readonly string _webserviceUrlSubmission;
        private readonly string _webserviceUrlRetrieval;

        public EUDRServiceAgent(EudrSettings settings)
        {
            _settings = settings;
            _webserviceUrlRetrieval = settings.ApiUrl + "/tracesnt/ws/EUDRRetrievalServiceV1";
            _webserviceUrlSubmission = settings.ApiUrl + "/tracesnt/ws/EUDRSubmissionServiceV1";
            Validate(settings);
        }

        #region Submission

        public async Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request)
        {
            EUDRSubmissionPortClient client = new EUDRSubmissionPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrlSubmission);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.submitDdsAsync(_settings.WebServiceClientId, request);

            return response;
        }

        #endregion Submission

        #region Retrieval

        public async Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<string> ddsIdentifiers)
        {
            EUDRRetrievalPortClient client = new EUDRRetrievalPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrlRetrieval);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.getDdsInfoAsync(_settings.WebServiceClientId, ddsIdentifiers.ToArray());

            IEnumerable<StatementInfoType> statements = new List<StatementInfoType>();
            if (response.GetStatementInfoResponse1 != null)
            {
                statements = response.GetStatementInfoResponse1;
            }

            return statements;
        }

        #endregion Retrieval

        #region Privates

        private static void Validate(EudrSettings settings)
        {
            if (settings == null)
            {
                throw new InvalidOperationException($"AppSettings: 'EudrSettings' section is missing or could not be bound correctly.");
            }
            if (string.IsNullOrEmpty(settings.AuthenticationKey)) throw new ApplicationException("Missing AuthenticationKey");
            if (string.IsNullOrEmpty(settings.Username)) throw new ApplicationException("Missing Username, not email");
            if (string.IsNullOrEmpty(settings.ApiUrl)) throw new ApplicationException("Missing ApiUrl");
            if (string.IsNullOrEmpty(settings.WebServiceClientId)) throw new ApplicationException("Missing WebServiceClientId");
        }

        #endregion Privates
    }
}