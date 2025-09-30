using Eudr.Traces.Integrations;
using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.Entities;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System.IO;
using System.Text;




// Bind settings
EudrSettings eudrSettings = new EudrSettings();
eudrSettings.ApiUrl = "https://acceptance.eudr.webcloud.ec.europa.eu";
eudrSettings.Username = "";
eudrSettings.AuthenticationKey = "";
eudrSettings.WebServiceClientId = "";

IEUDRServiceAgent agent = new EUDRServiceAgent(eudrSettings);

// Test setup
var requestGPS = new DDSWithGps
{
    EoriNumber = "SE5566778899",
    Name = "Test Operator AB",
    Street = "Mainstreet 1",
    PostalCode = "12345",
    City = "Alvesta",
    Country = "SE",
    Epost = "info@test.se",
    CommodityCode = "4407",
    CommodityGoodsDescription = "WOOD",
    CommodityGoodsWeight = 1500m,
    CommodityGoodsScientificName = "Pinus sylvestris",
    CommodityGoodsCommonName = "Scots Pine",
    InternalReferenceNumber = "7926588728",
    ActivityType = "IMPORT",
    EuCountryForActivity = "SE",
    ProducerCountry = "BR",
    ProducerName = "Fazenda Santa Ana",
    ProducerGpsPoints = [new GpsPoint() { Longitude = 14.536734, Latitude = 56.895696 }]

};
Console.WriteLine("Submit DDS");
var response = await agent.SubmitDdsAsync(requestGPS);
Console.WriteLine("Response DDS identifierkey " + response);

Console.WriteLine("Get DDS Status");
var responseStatus= await agent.GetDdsStatusAsync([response]);
Console.WriteLine("Response Status DDS" + response);