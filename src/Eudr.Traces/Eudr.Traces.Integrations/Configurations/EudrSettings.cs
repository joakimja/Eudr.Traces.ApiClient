using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Configurations
{
    public class EudrSettings
    {
        /// <summary>
        /// Username nn, not email
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// API key
        /// </summary>
        public string AuthenticationKey { get; set; } = string.Empty;
        /// <summary>
        /// WebServiceClientId client identifier
        /// </summary>
        public string WebServiceClientId { get; set; } = string.Empty;
        /// <summary>
        /// Url to environment i.e production https://eudr.webcloud.ec.europa.eu
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;
    }
}
