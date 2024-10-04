using FellowOakDicom;

namespace Api_PACsServer.Services.IService
{
    public interface IWadoResponseManager
    {
        /// <summary>
        /// Returns the image file as an HttpResponseMessage with the specified content type and transfer syntax.
        /// </summary>
        /// <param name="dicomFile">The DICOM file to be returned.</param>
        /// <param name="finalContentType">The final content type to use in the response.</param>
        /// <param name="transferSyntax">The transfer syntax to use within the DICOM image object.</param>
        /// <returns>An HttpResponseMessage containing the DICOM file in the specified content type and transfer syntax.</returns>
        HttpResponseMessage ReturnImageAsHttpResponse(DicomFile dicomFile, string finalContentType, string transferSyntax);
        /// <summary>
        /// Chooses the final content type given compatible content types by order of preference and the DICOM file.
        /// </summary>
        /// <param name="compatibleContentTypesByOrderOfPreference">A list of compatible content types by order of preference.</param>
        /// <param name="dicomFile">The DICOM file to be returned.</param>
        /// <returns>The chosen content type based on the compatibility and the DICOM file content.</returns>
        string PickFinalContentType(string[] compatibleContentTypesByOrderOfPreference, DicomFile dicomFile);
        /// <summary>
        /// Extracts content type values from the content type parameter string.
        /// </summary>
        /// <param name="contentType">The content type string from the WADO request.</param>
        /// <param name="contentTypes">An array of extracted content types.</param>
        /// <returns>True if the content types were successfully parsed, otherwise false.</returns>
        bool ExtractContentTypesFromContentTypeParameter(string contentType, out string[] contentTypes);
        /// <summary>
        /// Gets the compatible content types from the Accept header by order of preference.
        /// </summary>
        /// <param name="contentTypes">The content types specified in the request.</param>
        /// <param name="acceptContentTypesHeader">The content types specified in the Accept header.</param>
        /// <returns>An array of compatible content types by order of preference. If contentTypes is null, returns null.</returns>
        string[] GetCompatibleContentTypesByOrderOfPreference(string[] contentTypes, string[] acceptContentTypesHeader);
        /// <summary>
        /// Converts the DICOM transfer syntax string to a DicomTransferSyntax enumeration.
        /// </summary>
        /// <param name="transferSyntax">The transfer syntax string to be converted.</param>
        /// <returns>The corresponding DicomTransferSyntax enumeration. If the syntax is not supported, returns ExplicitVRLittleEndian by default.</returns>
        DicomTransferSyntax GetTransferSyntaxFromString(string transferSyntax);
    }
}
