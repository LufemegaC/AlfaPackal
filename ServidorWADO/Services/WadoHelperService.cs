using FellowOakDicom;
using FellowOakDicom.Imaging.Codec;
using FellowOakDicom.Imaging;
using ServidorWADO.Services.IService;
using System.Net;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;

namespace ServidorWADO.Services
{
    public class WadoHelperService : IWadoHelperService
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
        #region methods
        public bool ExtractContentTypesFromContentTypeParameter(string contentType, out string[] contentTypes)
        {
            //8.1.5 MIME type of the response 
            //The value shall be a list of MIME types, separated by a "," character, and potentially associated with
            //relative degree of preference, as specified in IETF RFC2616. 
            //so we must split the string

            contentTypes = null;
            if (contentType != null && contentType.Contains(","))
            {
                contentTypes = contentType.Split(',');
            }
            else if (contentType == null)
            {
                contentTypes = null;
            }
            else
            {
                contentTypes = new[] { contentType };
            }

            //we now need to parse each type which follows the RFC2616 syntax
            //it also extracts parameters like jpeg quality but we discard it because we don't need them for now
            try
            {
                if (contentType != null)
                {
                    contentTypes =
                        contentTypes.Select(contentTypeString => MediaTypeHeaderValue.Parse(contentTypeString))
                            .Select(mediaTypeHeader => mediaTypeHeader.MediaType).ToArray();
                }
            }
            catch (FormatException)
            {
                {
                    return false;
                }
            }
            return true;
        }

        public string[] GetCompatibleContentTypesByOrderOfPreference(string[] contentTypes, string[] acceptContentTypesHeader)
        {
            // Primero verifico que entre los tipos solicitados, haya uno que manejo.
            // Debo tomar la intersección de los tipos solicitados y aceptados y ordenarlos por orden de preferencia.

            bool acceptAllTypesInAcceptHeader = acceptContentTypesHeader.Contains("*/*");

            string[] compatibleContentTypesByOrderOfPreference;
            if (acceptAllTypesInAcceptHeader)
            {
                compatibleContentTypesByOrderOfPreference = contentTypes;
            }
            // null representa el valor predeterminado
            else if (contentTypes == null)
            {
                compatibleContentTypesByOrderOfPreference = null;
            }
            else
            {
                // intersect debe preservar el orden (por lo que ya está ordenado por orden de preferencia)
                compatibleContentTypesByOrderOfPreference = acceptContentTypesHeader.Intersect(contentTypes).ToArray();
            }
            return compatibleContentTypesByOrderOfPreference;
        }

        public DicomTransferSyntax GetTransferSyntaxFromString(string transferSyntax)
        {
            try
            {
                return DicomParseable.Parse<DicomTransferSyntax>(transferSyntax);
            }
            catch (Exception)
            {
                // Si tenemos un error, esto probablemente significa que la sintaxis no es compatible
                // Por lo tanto, según 8.2.11 en la especificación, usamos ExplicitVRLittleEndian por defecto
                return DicomTransferSyntax.ExplicitVRLittleEndian;
            }
        }

        public string PickFinalContentType(string[] compatibleContentTypesByOrderOfPreference, DicomFile dicomFile)
        {
            int nbFrames = dicomFile.Dataset.GetSingleValue<int>(DicomTag.NumberOfFrames);

            // Si compatibleContentTypesByOrderOfPreference es null,
            // significa que debemos elegir un tipo de contenido predeterminado basado en el contenido de la imagen:
            //  * Objetos de Imagen de Un Solo Fotograma
            //    Si el parámetro contentType no está presente en la solicitud, la respuesta debe contener un tipo MIME image/jpeg,
            //    si es compatible con el campo ‘Accept’ del método GET.
            //  * Objetos de Imagen de Varios Fotogramas
            //    Si el parámetro contentType no está presente en la solicitud, la respuesta debe contener un tipo MIME application/dicom.

            // No estoy seguro si esta es la forma de distinguir objetos de varios fotogramas.
            bool isMultiFrame = nbFrames > 1;
            bool chooseDefaultValue = compatibleContentTypesByOrderOfPreference == null;
            string chosenContentType;
            if (chooseDefaultValue)
            {
                chosenContentType = isMultiFrame ? AppDicomContentType : JpegImageContentType;
            }
            else
            {
                // Necesitamos tomar el compatible
                chosenContentType = compatibleContentTypesByOrderOfPreference
                    .Intersect(new[] { AppDicomContentType, JpegImageContentType })
                    .First();
            }
            return chosenContentType;
        }

        public HttpResponseMessage ReturnImageAsHttpResponse(DicomFile dicomFile, string finalContentType, string transferSyntax)
        {
            MediaTypeHeaderValue header = null;
            Stream streamContent = null;

            if (finalContentType == JpegImageContentType)
            {
                var image = new DicomImage(dicomFile.Dataset);
                Bitmap bmp = image.RenderImage(0).As<Bitmap>();

                // Cuando se devuelve un tipo MIME image/jpeg, la imagen debe codificarse utilizando el proceso de codificación con pérdidas JPEG de 8 bits basado en Huffman sin jerarquía y no secuencial ISO/IEC 10918.
                header = new MediaTypeHeaderValue(JpegImageContentType);
                streamContent = new MemoryStream();

                bmp.Save(streamContent, ImageFormat.Jpeg);
                streamContent.Seek(0, SeekOrigin.Begin);
            }
            else if (finalContentType == AppDicomContentType)
            {
                // Por defecto, la sintaxis de transferencia debe ser "Explicit VR Little Endian".
                // Nota: Esto implica que las imágenes recuperadas se envían sin comprimir por defecto.
                DicomTransferSyntax requestedTransferSyntax = DicomTransferSyntax.ExplicitVRLittleEndian;

                if (transferSyntax != null)
                {
                    requestedTransferSyntax = GetTransferSyntaxFromString(transferSyntax);
                }

                bool transferSyntaxIsTheSameAsSourceFile = dicomFile.FileMetaInfo.TransferSyntax == requestedTransferSyntax;

                // Solo cambiamos la sintaxis de transferencia si es necesario
                DicomFile dicomFileToStream = !transferSyntaxIsTheSameAsSourceFile ? dicomFile.Clone(requestedTransferSyntax) : dicomFile;
                header = new MediaTypeHeaderValue(AppDicomContentType);
                streamContent = new MemoryStream();
                dicomFileToStream.Save(streamContent);
                streamContent.Seek(0, SeekOrigin.Begin);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(streamContent)
            };
            result.Content.Headers.ContentType = header;
            return result;
        }
        #endregion
    }
}
