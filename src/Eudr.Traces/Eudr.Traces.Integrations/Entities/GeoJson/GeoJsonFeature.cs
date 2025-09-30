using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities.GeoJson
{
    public class GeoJsonFeature
    {
        public string type { get; set; } = "Feature";
        public GeoJsonGeometry? geometry { get; set; }
        public object? properties { get; set; }
    }
}
