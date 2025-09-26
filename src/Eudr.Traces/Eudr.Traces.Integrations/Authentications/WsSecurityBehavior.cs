

using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Eudr.Traces.Integrations.Authentications
{
    public class WsSecurityBehavior : IEndpointBehavior
    {
        private readonly string _username;
        private readonly string _authenticationKey;

        public WsSecurityBehavior(string username, string authenticationKey)
        {
            _username = username;
            _authenticationKey = authenticationKey;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new WsSecurityMessageInspector(_username, _authenticationKey));
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }

}