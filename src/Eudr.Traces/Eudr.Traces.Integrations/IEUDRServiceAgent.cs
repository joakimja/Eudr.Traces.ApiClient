using Eudr.Traces.Integrations.Entities;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRRetrievalService;
using Eudr.Traces.Integrations.ServiceAgents.Proxys.EUDRSubmissionService;

namespace Eudr.Traces.Integrations
{
    /// <summary>
    /// Interface for communicating with TRACES regarding EUDR operations.
    /// </summary>
    public interface IEUDRServiceAgent
    {
        /// <summary>
        /// Retrieves the status of one or more DDS (Due Diligence Statements).
        /// </summary>
        /// <param name="ddsIdentifiers">A collection of GUID identifiers for the DDS to retrieve status for.</param>
        /// <returns>A task that resolves to a collection of DDS status objects.</returns>
        Task<IEnumerable<StatementInfoType>> GetDdsStatusAsync(IEnumerable<Guid> ddsIdentifiers);

        /// <summary>
        /// Retrieves a single DDS using its reference and verification numbers.
        /// </summary>
        /// <param name="reference">The reference number of the DDS.</param>
        /// <param name="verification">The verification number of the DDS.</param>
        /// <returns>A task that resolves to the DDS details.</returns>
        Task<GetStatementByIdentifiersResponse> GetStatementByIdentifiersAsync(string reference, string verification);

        /// <summary>
        /// Submits a new DDS. Can be used for both GPS-based or reference-based DDS. For GPS, only a single point is required.
        /// </summary>
        /// <param name="request">The complete DDS submission request.</param>
        /// <returns>A task that resolves to the submission response including status and identifiers.</returns>
        Task<SubmitStatementResponse> SubmitDdsAsync(SubmitStatementRequestType request);

        /// <summary>
        /// Submits a simplified DDS using a GPS point or a area.
        /// </summary>
        /// <param name="request">The DDS data containing a GPS point.</param>
        /// <returns>A task that resolves to the identifier (GUID) of the submitted DDS.</returns>
        Task<Guid> SubmitDdsAsync(DDSWithGps request);

        /// <summary>
        /// Submits a simplified DDS that references other existing DDS.
        /// </summary>
        /// <param name="request">The DDS data containing references to other DDS.</param>
        /// <returns>A task that resolves to the identifier (GUID) of the submitted DDS.</returns>
        Task<Guid> SubmitDdsAsync(DDSWithReference request);

        /// <summary>
        /// Validates whether a DDS is in an acceptable state for use, based on defined business logic.
        /// </summary>
        /// <param name="reference">The reference number of the DDS.</param>
        /// <param name="verification">The verification number of the DDS.</param>
        /// <returns>A task that returns true if the DDS is valid for use, otherwise false.</returns>
        Task<bool> ValidateDDSAsync(string reference, string verification);
    }

}