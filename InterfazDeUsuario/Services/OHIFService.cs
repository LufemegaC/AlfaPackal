using InterfazDeUsuario.Models.visorOHIF;
using InterfazDeUsuario.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace InterfazDeUsuario.Services
{
    public class OHIFService : IOHIFService
    {
        public async Task SendStudyDataAsync(OHIFStudy studyInfo)
        {
            // Serializa el objeto OHIFStudy a JSON
            string studyInfoJson = JsonConvert.SerializeObject(studyInfo);

            // Aquí puedes enviar el JSON al visor OHIF. 
            // Dependiendo de cómo lo estés implementando, puedes usar HttpClient o el método que prefieras.
            await EnviarJsonAlVisorOHIF(studyInfoJson);
        }

        private async Task EnviarJsonAlVisorOHIF(string jsonData)
        {
            // Implementa la lógica para enviar el JSON al visor OHIF, por ejemplo, con HttpClient
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Aquí haces el envío sin esperar una respuesta importante
                await client.PostAsync("http://url-del-visor-ohif", content);
            }
        }
    }
}
