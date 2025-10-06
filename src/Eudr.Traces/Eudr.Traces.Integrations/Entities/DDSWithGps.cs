using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities
{
    /// <summary>
    /// Simple class to add a DDS with gps
    /// </summary>
    public class DDSWithGps : DDS
    {
        public DDSWithGps()
        {
            ProducerGpsPoints = new List<GpsPoint>();
        }



        /// <summary>
        /// Name of the producer or farm where the commodity was sourced.
        /// </summary>
        public string ProducerName { get; set; } = string.Empty;

        /// <summary>
        /// ISO country code where the producer is located (e.g., "BR" for Brazil).
        /// </summary>
        public string ProducerCountry { get; set; } = string.Empty;

        /// <summary>
        /// Geolocation points (as polygons or single point) where the production took place.
        /// If one point, use Point. If multiple, it's an Area.
        /// </summary>
        public IEnumerable<GpsPoint> ProducerGpsPoints { get; set; }


    }
}