using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities.GeoJson
{
    public class GeoJsonFeatureCollection
    {
        public string type { get; set; } = "FeatureCollection";
        public List<GeoJsonFeature> features { get; set; } = new();
    }
}
