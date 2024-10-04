using InterfazBasica.Models;

namespace InterfazBasica_DCStore.Models.Dicom.Web
{
    public class DicomAPIRequest : APIRequest
    {
        public new MultipartContent RequestData { get; set; }
    }
}
