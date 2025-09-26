using Eudr.Traces.Integrations.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Extensions
{
    public static class SettingExtensions
    {
        public static void Validate(this EudrSettings settings)
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
    }
}
