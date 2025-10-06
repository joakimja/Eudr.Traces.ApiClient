using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities
{
    /// <summary>
    /// Simple class to add a DDS with reference and verification
    /// </summary>
    public class DDSWithReference : DDS
    {
        public DDSWithReference()
        {
            References = new List<(string reference, string verification)>();
        }
        /// <summary>
        /// reference and verification
        /// </summary>
        public IEnumerable<(string reference, string verification)> References { get; set; }
    }
}
