using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.ServiceAgents;
using Eudr.Traces.Integrations.ServiceAgents.Interfaces;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;


// Setup Configuration
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// Bind settings
var settings = config.GetSection("EudrSettings").Get<EudrSettings>();
IEUDRSubmissionServiceAgent agent = new EUDRSubmissionServiceAgent(settings);

// Test setup
// Add and get status
var statement = new DueDiligenceStatementBaseType
{
    internalReferenceNumber = "7926588728",
    activityType = ActivityType.IMPORT,
    @operator = new EconomicOperatorIdentificationType
    {
        nameAndAddress = new NameAndAddressType
        {
            name = "Testsson",
            country = "BE",
            address = "Main Street 1"
        },
        email = "test@example.com"
    },
    countryOfActivity = CountryType.BE,

    commodities = new CommodityType[]
    {
        new CommodityType
        {
            descriptors = new CommercialDescriptionType
            {
                descriptionOfGoods = "WOOD",
                goodsMeasure = new GoodsMeasureType
                {
                    volume = 2500,
                    volumeSpecified = true,
                    netWeight = 500,
                    netWeightSpecified = true
                }
            },
            hsHeading = "4407" ,
            speciesInfo = new[]
            {
                new SpeciesInformationType
                {
                    scientificName = "Pinus sylvestris",
                    commonName = "Scots Pine"
                }
            },
            producers = new[]
            {
                new ProducerType
                {
                    country = "BR",
                    name = "Fazenda Santa Ana",
                    geometryGeojson = Encoding.UTF8.GetBytes(@"
                    {
                      ""type"": ""FeatureCollection"",
                      ""features"": [{
                        ""type"": ""Feature"",
                        ""geometry"": {
                          ""type"": ""Point"",
                          ""coordinates"": [14.536734, 56.895696]
                        },
                        ""properties"": {}
                      }]
                    }")
                }
            }
        },
    }
};

var request = new SubmitStatementRequestType
{
    operatorType = OperatorActivityType.OPERATOR,
    statement = statement
};
Console.WriteLine("Submit DDS");
var response = await agent.SubmitDdsAsync(request);
Console.WriteLine("Response DDS" + response.SubmitStatementResponse1.ddsIdentifier);

IEUDRRetrievalServiceAgent agentRetrieval = new EUDRRetrievalServiceAgent(settings);
var responseStatus= await agentRetrieval.GetDdsStatusAsync([response.SubmitStatementResponse1.ddsIdentifier]);
Console.WriteLine("Response Status DDS" + response);