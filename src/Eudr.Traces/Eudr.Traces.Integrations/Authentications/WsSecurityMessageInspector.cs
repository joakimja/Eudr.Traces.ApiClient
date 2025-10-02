using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.ServiceModel;

namespace Eudr.Traces.Integrations.Authentications
{
    public class WsSecurityMessageInspector : IClientMessageInspector
    {
        private readonly string _username;
        private readonly string _authenticationKey;

        public WsSecurityMessageInspector(string username, string authenticationKey)
        {
            _username = username;
            _authenticationKey = authenticationKey;
        }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var securityHeaderXml = CreateWorkingSecurityHeader();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(securityHeaderXml);

            var securityElement = xmlDoc.DocumentElement;
            if (securityElement != null) {
                request.Headers.Add(new XmlElementMessageHeader(securityElement));
            }
            

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        { }

        private string CreateWorkingSecurityHeader()
        {
            string created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            byte[] nonceBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonceBytes);
            string nonceB64 = Convert.ToBase64String(nonceBytes);

            byte[] createdBytes = Encoding.UTF8.GetBytes(created);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(_authenticationKey);
            byte[] concat = new byte[nonceBytes.Length + createdBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(nonceBytes, 0, concat, 0, nonceBytes.Length);
            Buffer.BlockCopy(createdBytes, 0, concat, nonceBytes.Length, createdBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, concat, nonceBytes.Length + createdBytes.Length, passwordBytes.Length);

            using var sha1 = SHA1.Create();
            string passwordDigest = Convert.ToBase64String(sha1.ComputeHash(concat));

            return $@"<wsse:Security
                      xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'
                      xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                      <wsu:Timestamp wsu:Id='Timestamp-{Guid.NewGuid()}'>
                        <wsu:Created>{created}</wsu:Created>
                        <wsu:Expires>{DateTime.UtcNow.AddMinutes(1):yyyy-MM-ddTHH:mm:ssZ}</wsu:Expires>
                      </wsu:Timestamp>
                      <wsse:UsernameToken wsu:Id='UsernameToken-{Guid.NewGuid()}'>
                        <wsse:Username>{_username}</wsse:Username>
                        <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest'>{passwordDigest}</wsse:Password>
                        <wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>{nonceB64}</wsse:Nonce>
                        <wsu:Created>{created}</wsu:Created>
                      </wsse:UsernameToken>
                    </wsse:Security>";
        }
    }
}