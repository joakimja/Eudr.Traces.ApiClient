using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities
{
    /// <summary>
    /// Used as point or point array to fence a area
    /// </summary>
    public class GpsPoint
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
