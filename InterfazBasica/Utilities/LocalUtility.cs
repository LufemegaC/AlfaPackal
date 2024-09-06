namespace InterfazBasica_DCStore.Utilities
{
    public static class LocalUtility
    {
        public static string StorageURL = "C:\\Users\\Desktop\\source\\repos\\AlfaPackal\\DicomArchives";
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static string GetLocalIPAddress()
        {
            // NOTA IMPORTANTE(05/07/24):
            // ESTE CODIGO SERA COMENTADO DEBIDO A QUE LA IP 
            // SE CAMBIA CONSTANTEMENTE, COMO PALIATIVO SE DEVOLVERA
            // UN VALOR CONSTANTE

            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ipAddress in host.AddressList)
            //{
            //    if (ipAddress.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipAddress))
            //    {
            //        return ipAddress.ToString();
            //    }
            //}
            //return "IP no encontrada";

            return "192.168.1.64";
        }

        public static string Aetitle;

        public static string SessionToken = "JWToken";

        public static int InstitutionId;

        public static string Institution = "Institution";


    }
}
