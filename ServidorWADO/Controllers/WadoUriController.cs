using FellowOakDicom;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServidorWADO.Services.IService;
using System.Diagnostics;
using System.Net;
using System.Web.Http;


namespace ServidorWADO.Controllers
{
    /// <summary>
    /// main controller for wado implementation
    /// </summary>
    /// <remarks>the current implementation is incomplete</remarks>
    /// <remarks>more infos in the official specification : http://dicom.nema.org/medical/dicom/current/output/pdf/part18.pdf </remarks>
    [System.Web.Http.Route("api/[controller]")]
    [EnableCors("WadoCorsPolicy")]
    [RoutePrefix("wado")]
    [ApiController]
    public class WadoUriController : ControllerBase
    {
        #region consts

        /// <summary>
        /// string representation of the dicom content type
        /// </summary>
        private const string AppDicomContentType = "application/dicom";

        /// <summary>
        /// string representation of the jpeg content type
        /// </summary>
        private const string JpegImageContentType = "image/jpeg";
        #endregion

        #region fields

        /// <summary>
        /// service used to retrieve images by instance Uid
        /// </summary>
        private readonly IDicomImageFinderService _dicomImageFinderService;
        /// <summary>
        /// service used to help in validation process
        /// </summary>
        private readonly IWadoHelperService _wadoHelperService;

        #endregion

        public WadoUriController(IWadoHelperService wadoHelperService, IDicomImageFinderService dicomImageFinderService)
        {
            _dicomImageFinderService = dicomImageFinderService;
            _wadoHelperService = wadoHelperService;
        }

        #region methods

        /// <summary>
        /// main wado method
        /// </summary>
        /// <param name="requestMessage">web request</param>
        /// <param name="requestType">always equals to wado in current wado specification, may change in the future</param>
        /// <param name="studyUID">study instance UID</param>
        /// <param name="seriesUID">serie instance UID</param>
        /// <param name="objectUID">instance UID</param>
        /// <param name="contentType">The value shall be a list of MIME types, separated by a "," character, and potentially associated with relative degree of preference, as specified in IETF RFC2616. </param>
        /// <param name="charset">character set of the object to be retrieved.</param>
        /// <param name="transferSyntax">The Transfer Syntax to be used within the DICOM image object, as specified in PS 3.6</param>
        /// <param name="anonymize">if value is "yes", indicates that we should anonymize object.
        /// The Server may return an error if it either cannot or refuses to anonymize that object</param>
        /// <returns></returns>
        
        //[Route("")]
        public async Task<HttpResponseMessage> GetStudyInstances(HttpRequestMessage requestMessage, string requestType,
            string studyUID, string seriesUID, string objectUID, string contentType = null, string charset = null,
            string transferSyntax = null, string anonymize = null)
        {

            //we do not handle anonymization
            if (anonymize == "yes")
            {
                return requestMessage.CreateErrorResponse(HttpStatusCode.NotAcceptable, "anonymise is not supported on the server");
            }

            //we extract the content types from contentType value
            bool canParseContentTypeParameter = _wadoHelperService.ExtractContentTypesFromContentTypeParameter(contentType,
                out string[] contentTypes);

            if (!canParseContentTypeParameter)
            {
                return requestMessage.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                    string.Format("contentType parameter (value: {0}) cannot be parsed", contentType));
            }


            //8.1.5 The Web Client shall provide list of content types it supports in the "Accept" field of the GET method. The
            //value of the contentType parameter of the request shall be one of the values specified in that field. 
            string[] acceptContentTypesHeader =
                requestMessage.Headers.Accept.Select(header => header.MediaType).ToArray();

            // */* means that we accept everything for the content Header
            bool acceptAllTypesInAcceptHeader = acceptContentTypesHeader.Contains("*/*");
            bool isRequestedContentTypeCompatibleWithAcceptContentHeader = acceptAllTypesInAcceptHeader ||
                                                                           contentTypes == null ||
                                                                           acceptContentTypesHeader.Intersect(
                                                                               contentTypes).Any();

            if (!isRequestedContentTypeCompatibleWithAcceptContentHeader)
            {
                return requestMessage.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                    string.Format("content type {0} is not compatible with types specified in  Accept Header",
                        contentType));
            }

            //6.3.2.1 The MIME type shall be one on the MIME types defined in the contentType parameter, preferably the most
            //desired by the Web Client, and shall be in any case compatible with the ‘Accept’ field of the GET method.
            //Note: The HTTP behavior is that an error (406 – Not Acceptable) is returned if the required content type cannot
            //be served. 
            string[] compatibleContentTypesByOrderOfPreference =
                _wadoHelperService.GetCompatibleContentTypesByOrderOfPreference(contentTypes,
                    acceptContentTypesHeader);

            //if there is no type that can be handled by our server, we return an error
            if (compatibleContentTypesByOrderOfPreference != null
                && !compatibleContentTypesByOrderOfPreference.Contains(JpegImageContentType)
                && !compatibleContentTypesByOrderOfPreference.Contains(AppDicomContentType))
            {
                return requestMessage.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                    string.Format("content type(s) {0} cannot be served",
                        string.Join(" - ", compatibleContentTypesByOrderOfPreference)
                        ));
            }

            //we now need to handle the case where contentType is not specified, but in this case, the default value
            //depends on the image, so we need to open it

            string dicomImagePath = _dicomImageFinderService.GetImageByInstanceUid(objectUID);

            if (dicomImagePath == null)
            {
                return requestMessage.CreateErrorResponse(HttpStatusCode.NotFound, "no image found");
            }

            try
            {
                DicomFile dicomFile = await DicomFile.OpenAsync(dicomImagePath);

                string finalContentType = _wadoHelperService.PickFinalContentType(compatibleContentTypesByOrderOfPreference, dicomFile);

                return _wadoHelperService.ReturnImageAsHttpResponse(dicomFile,
                    finalContentType, transferSyntax);
            }
            catch (Exception ex)
            {
                Trace.TraceError("exception when sending image: " + ex.ToString());

                return requestMessage.CreateErrorResponse(HttpStatusCode.InternalServerError, "server internal error");
            }
        }
        #endregion
    }
}
