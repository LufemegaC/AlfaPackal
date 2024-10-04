using System.Net.Sockets;
using System.Net;

namespace WorkStation.Utilities
{
    public static class LocalUtility
    {

        public static string GetLocalIPAddress_Fake()
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

        public static string GetLocalIPAddress_Real()
        {

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipAddress))
                {
                    return ipAddress.ToString();
                }
            }
            return "IP no encontrada";
        }

        public static int GetServerPort(int puerto)
        {
            return (puerto == 0) ? 11112 : puerto;
        }
    }
}
