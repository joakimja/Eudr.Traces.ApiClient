To generate a new proxy use this 

dotnet-svcutil https://acceptance.eudr.webcloud.ec.europa.eu/tracesnt/ws/EUDRSubmissionServiceV1?wsdl --serializer XmlSerializer -n "*",Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService --outputFile EUDRSubmissionServiceV1.cs