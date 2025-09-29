To generate a new proxy use this

dotnet-svcutil https://acceptance.eudr.webcloud.ec.europa.eu/tracesnt/ws/EUDRSubmissionServiceV1?wsdl --serializer XmlSerializer -n "\*",Eudr.Traces.Integrations.Proxys.EUDRSubmissionService --outputFile EUDRSubmissionServiceV1.cs

dotnet-svcutil https://acceptance.eudr.webcloud.ec.europa.eu/tracesnt/ws/EUDRRetrievalServiceV1?wsdl --serializer XmlSerializer -n "\*",Eudr.Traces.Integrations.Proxys.EUDRRetrievalService --outputFile EUDRRetrievalServiceV1.cs

