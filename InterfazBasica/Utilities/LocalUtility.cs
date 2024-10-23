using FellowOakDicom.Network;
using InterfazBasica.Models;
using System.Net;

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

        public enum StorageLocation
        {
            Permanent,   // Para almacenar de manera permanente (rootPath)
            Temporary,    // Para almacenamiento temporal (requestPath)
            Failed
        }

        public static DicomStatus TranslateApiResponseToDicomStatus(APIResponse apiResponse)
        {
            // Caso de éxito
            if (apiResponse.StatusCode == HttpStatusCode.OK && apiResponse.IsSuccessful)
            {
                return DicomStatus.Success;
            }

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return DicomStatus.InvalidArgumentValue;
                case HttpStatusCode.NotFound:
                    return DicomStatus.NoSuchObjectInstance;
                case HttpStatusCode.InternalServerError:
                    return DicomStatus.ProcessingFailure;
                case HttpStatusCode.ServiceUnavailable:
                    return DicomStatus.ResourceLimitation;
                case HttpStatusCode.Accepted:
                    return DicomStatus.Warning;
                default:
                    // Un mapeo genérico para otros casos
                    return DicomStatus.ProcessingFailure;
            }
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
