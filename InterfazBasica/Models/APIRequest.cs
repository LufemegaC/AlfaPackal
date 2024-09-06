using static InterfazBasica_DCStore.Utilities.LocalUtility;


namespace InterfazBasica.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object RequestData { get; set; }
        public string Token { get; set; }
    }
}
