using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities.GeoJson
{
    public class GeoJsonGeometry
    {
        public string type { get; set; } = string.Empty;  // "Point", "Polygon"
        public object? coordinates { get; set; }
    }
}
