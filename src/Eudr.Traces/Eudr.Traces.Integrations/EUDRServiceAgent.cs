using Eudr.Traces.Integrations.Authentications;
using Eudr.Traces.Integrations.Configurations;
using Eudr.Traces.Integrations.Entities;
using Eudr.Traces.Integrations.Entities.GeoJson;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;
using System.Reflection.Metadata.Ecma335;
using System.ServiceModel;
using System.Text;
using System.Text.Json;

namespace Eudr.Traces.Integrations
{
    public class EUDRServiceAgent : IEUDRServiceAgent
    {
        private readonly EudrSettings _settings;

        private readonly string _webserviceUrlSubmission;
        private readonly string _webserviceUrlRetrieval;

        public EUDRServiceAgent(EudrSettings settings)
        {
            _settings = settings;
            _webserviceUrlRetrieval = settings.ApiUrl + "/tracesnt/ws/EUDRRetrievalServiceV1";
            _webserviceUrlSubmission = settings.ApiUrl + "/tracesnt/ws/EUDRSubmissionServiceV1";
            Validate(settings);
        }

        #region Simple versions

        public async Task<Guid> SubmitDdsAsync(DDSWithGps request)
        {
            Guid retval = Guid.Empty;
            SubmitStatementRequestType submit = new SubmitStatementRequestType();
            submit = MapSubmitDDSWithGPS(request);

            var response = await SubmitDdsAsync(submit);
            if (response != null &&
                response.SubmitStatementResponse1 != null &&
                response.SubmitStatementResponse1.ddsIdentifier != null)
            {
                retval = Guid.Parse(response.SubmitStatementResponse1.ddsIdentifier);
            }
            return retval;
        }

        public async Task<Guid> SubmitDdsAsync(DDSWithReference request)
        {
            Guid retval = Guid.Empty;
            SubmitStatementRequestType submit = new SubmitStatementRequestType();
            submit = MapSubmitDDSWithReference(request);
            var response = await SubmitDdsAsync(submit);
            if (response != null &&
                response.SubmitStatementResponse1 != null &&
                response.SubmitStatementResponse1.ddsIdentifier != null)
            {
                retval = Guid.Parse(response.SubmitStatementResponse1.ddsIdentifier);
            }
            return retval;
        }

        public async Task<bool> ValidateDDSAsync(string reference, string verification)
        {
            bool retval = false;
            List<DdsStatusType> validStatues = [DdsStatusType.AVAILABLE, DdsStatusType.ARCHIVED];
            var response = await GetStatementByIdentifiersAsync(reference, verification);

            if (response.statement != null && validStatues.Contains(response.statement.status.status))
            {
                retval = true;
            }
            return retval;
        }

        #endregion Simple versions

        #region Submission

        public async Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request)
        {
            EUDRSubmissionPortClient client = new EUDRSubmissionPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrlSubmission);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.submitDdsAsync(_settings.WebServiceClientId, request);

            return response;
        }

        #endregion Submission

        #region Retrieval

        public async Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<Guid> ddsIdentifiers)
        {
            EUDRRetrievalPortClient client = new EUDRRetrievalPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrlRetrieval);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var response = await client.getDdsInfoAsync(_settings.WebServiceClientId, ddsIdentifiers.Select(g => g.ToString()).ToArray());

            IEnumerable<StatementInfoType> statements = new List<StatementInfoType>();
            if (response.GetStatementInfoResponse1 != null)
            {
                statements = response.GetStatementInfoResponse1;
            }

            return statements;
        }

        public async Task<GetStatementByIdentifiersResponse> GetStatementByIdentifiersAsync(string reference, string verification)
        {
            EUDRRetrievalPortClient client = new EUDRRetrievalPortClient();
            client.Endpoint.Address = new EndpointAddress(_webserviceUrlRetrieval);
            client.Endpoint.EndpointBehaviors.Add(new WsSecurityBehavior(_settings.Username, _settings.AuthenticationKey));

            var request = new GetStatementByIdentifiersRequest
            {
                referenceNumber = reference,
                verificationNumber = verification
            };

            var response = await client.getStatementByIdentifiersAsync(_settings.WebServiceClientId, request);

            GetStatementByIdentifiersResponse statementByIdentifiersResponse = new GetStatementByIdentifiersResponse();
            if (response.GetStatementByIdentifiersResponse != null)
            {
                statementByIdentifiersResponse = response.GetStatementByIdentifiersResponse;
            }

            return statementByIdentifiersResponse;
        }

        #endregion Retrieval

        #region Privates

        private SubmitStatementRequestType MapSubmitDDSWithReference(DDSWithReference request)
        {
            var activityType = Enum.Parse<ServiceAgents.Proxys.EUDRSubmissionService.ActivityType>(request.ActivityType);
            var euCountryForActivity = Enum.Parse<ServiceAgents.Proxys.EUDRSubmissionService.CountryType>(request.EuCountryForActivity);
            var operatorType = Enum.Parse<OperatorActivityType>(request.OperatorType);

            var statement = new DueDiligenceStatementBaseType
            {
                internalReferenceNumber = request.InternalReferenceNumber,
                activityType = activityType,

                @operator = new EconomicOperatorIdentificationType
                {
                    nameAndAddress = new NameAndAddressType
                    {
                        name = request.Name,
                        street = request.Street,
                        postalCode = request.PostalCode,
                        city = request.City,
                        country = request.Country,
                        address = $"{request.Name}, {request.Street}, {request.PostalCode} {request.City}, {request.Country}"
                    },
                    email = request.Epost,
                    referenceNumber = new[]
                                     {
                                        new EconomicOperatorReferenceNumberType
                                        {
                                            identifierType = IdentifierTypeType.eori, // enumvalue
                                            identifierValue = request.EoriNumber
                                        }
                                    },
                },
                countryOfActivity = euCountryForActivity,

                associatedStatements = request.References
                    .Select(r => new AssociatedStatementsType
                    {
                        referenceNumber = r.reference,
                        verificationNumber = r.verification
                    })
                    .ToArray(),

                commodities = new ServiceAgents.Proxys.EUDRSubmissionService.CommodityType[]
                 {
                new ServiceAgents.Proxys.EUDRSubmissionService.CommodityType
                {
                    descriptors = new ServiceAgents.Proxys.EUDRSubmissionService.CommercialDescriptionType
                    {
                        descriptionOfGoods =request.CommodityGoodsDescription,
                        goodsMeasure = new ServiceAgents.Proxys.EUDRSubmissionService.GoodsMeasureType
                        {
                            netWeight = request.CommodityGoodsWeight,
                            netWeightSpecified = true
                        }
                    },
                    hsHeading = request.CommodityCode ,
                    speciesInfo = new[]
                    {
                        new ServiceAgents.Proxys.EUDRSubmissionService.SpeciesInformationType
                        {
                            scientificName = request.CommodityGoodsScientificName,
                            commonName = request.CommodityGoodsCommonName
                        }
                    },
                },
                }
            };

            var retval = new SubmitStatementRequestType
            {
                operatorType = operatorType,
                statement = statement
            };

            return retval;
        }

        private SubmitStatementRequestType MapSubmitDDSWithGPS(DDSWithGps request)
        {
            var producer = new ServiceAgents.Proxys.EUDRSubmissionService.ProducerType
            {
                country = request.ProducerCountry,
                name = request.ProducerName,
            };
            producer.geometryGeojson = GetGeoJsonBytes(request.ProducerGpsPoints);
            var activityType = Enum.Parse<ServiceAgents.Proxys.EUDRSubmissionService.ActivityType>(request.ActivityType);
            var euCountryForActivity = Enum.Parse<ServiceAgents.Proxys.EUDRSubmissionService.CountryType>(request.EuCountryForActivity);
            var operatorType = Enum.Parse<OperatorActivityType>(request.OperatorType);

            var statement = new DueDiligenceStatementBaseType
            {
                internalReferenceNumber = request.InternalReferenceNumber,
                activityType = activityType,

                @operator = new EconomicOperatorIdentificationType
                {
                    nameAndAddress = new NameAndAddressType
                    {
                        name = request.Name,
                        street = request.Street,
                        postalCode = request.PostalCode,
                        city = request.City,
                        country = request.Country,
                        address = $"{request.Name}, {request.Street}, {request.PostalCode} {request.City}, {request.Country}"
                    },
                    email = request.Epost,
                    referenceNumber = new[]
                                     {
                                        new EconomicOperatorReferenceNumberType
                                        {
                                            identifierType = IdentifierTypeType.eori, // enumvalue
                                            identifierValue = request.EoriNumber
                                        }
                                    },
                },
                countryOfActivity = euCountryForActivity,

                commodities = new ServiceAgents.Proxys.EUDRSubmissionService.CommodityType[]
                 {
                new ServiceAgents.Proxys.EUDRSubmissionService.CommodityType
                {
                    descriptors = new ServiceAgents.Proxys.EUDRSubmissionService.CommercialDescriptionType
                    {
                        descriptionOfGoods =request.CommodityGoodsDescription,
                        goodsMeasure = new ServiceAgents.Proxys.EUDRSubmissionService.GoodsMeasureType
                        {
                            netWeight = request.CommodityGoodsWeight,
                            netWeightSpecified = true
                        }
                    },
                    producers=[producer],
                    hsHeading = request.CommodityCode ,
                    speciesInfo = new[]
                    {
                        new ServiceAgents.Proxys.EUDRSubmissionService.SpeciesInformationType
                        {
                            scientificName = request.CommodityGoodsScientificName,
                            commonName = request.CommodityGoodsCommonName
                        }
                    },
                },
            }
            };

            var retval = new SubmitStatementRequestType
            {
                operatorType = operatorType,
                statement = statement
            };

            return retval;
        }

        private static Byte[] GetGeoJsonBytes(IEnumerable<GpsPoint> gpsPoints)
        {
            GeoJsonGeometry geometry = new GeoJsonGeometry();
            if (gpsPoints.Count() == 1)
            {
                var gpsPoint = gpsPoints.First();
                geometry = new GeoJsonGeometry
                {
                    type = "Point",
                    coordinates = new double[] { gpsPoint.Longitude, gpsPoint.Latitude } // [lon, lat]
                };
            }
            else if (gpsPoints.Count() > 1)
            {
                double[][] gpsArea = gpsPoints.Select(p => new double[] { p.Longitude, p.Latitude }).ToArray();

                geometry = new GeoJsonGeometry
                {
                    type = "Polygon",
                    coordinates = new double[][][] { gpsArea }
                };
            }
            else
            {
                throw new ApplicationException("Missing gpspoints");
            }

            var featureCollection = new GeoJsonFeatureCollection
            {
                features = new List<GeoJsonFeature>
                {
                    new GeoJsonFeature
                    {
                        geometry = geometry,
                        properties = new { } // empty properties
                    }
                }
            };

            // Serialize to JSON
            string geoJson = JsonSerializer.Serialize(featureCollection, new JsonSerializerOptions { WriteIndented = true });

            // // Convert to byte[]
            byte[] geometryGeojson = Encoding.UTF8.GetBytes(geoJson);
            return geometryGeojson;
        }

        private static void Validate(EudrSettings settings)
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

        #endregion Privates
    }
}