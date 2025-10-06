using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eudr.Traces.Integrations.Entities
{
    public abstract class DDS
    {
        public string OperatorType { get; set; } = "OPERATOR";
        /// <summary>
        /// EORI number of the operator (e.g., "SE5566778899").
        /// </summary>
        public string EoriNumber { get; set; } = string.Empty;

        /// <summary>
        /// Name of the economic operator (company or individual).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Street address of the operator (e.g., "Mainstreet 1").
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Postal code of the operator's address.
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// City of the operator's address.
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Country code (ISO 2-letter) of the operator (e.g., "SE").
        /// </summary>
        public string Country { get; set; } = string.Empty;
        /// <summary>
        /// Epost to operator
        /// </summary>
        public string Epost { get; set; } = string.Empty;

        /// <summary>
        /// The HS heading code for the commodity (e.g., "440799").
        /// </summary>
        public string CommodityCode { get; set; } = string.Empty;

        /// <summary>
        /// General description of the commodity (e.g., "Processed wood panels").
        /// </summary>
        public string CommodityGoodsDescription { get; set; } = string.Empty;


        /// <summary>
        /// Weight or volume of the goods (e.g., "1500 kg").
        /// </summary>
        public decimal CommodityGoodsWeight { get; set; }

        /// <summary>
        /// Scientific name of the species used in the commodity (e.g., "Swietenia macrophylla").
        /// </summary>
        public string CommodityGoodsScientificName { get; set; } = string.Empty;

        /// <summary>
        /// Common/trade name of the species (e.g., "Mahogany").
        /// </summary>
        public string CommodityGoodsCommonName { get; set; } = string.Empty;

        /// <summary>
        /// Internal reference
        /// </summary>
        public string InternalReferenceNumber { get; set; } = string.Empty;
        /// <summary>
        /// Activity for DDS
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;
        /// <summary>
        /// Valid EU Country for activity
        /// </summary>
        public string EuCountryForActivity { get; set; } = string.Empty;
    }
}
