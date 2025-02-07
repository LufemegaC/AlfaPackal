using InterfazDeUsuario.Models;
using InterfazDeUsuario.Services.IDicomWeb;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Services
{
    public class DicomWebService : DicomWebBaseService, IDicomWebService
    {
        //Crear Serie
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public DicomWebService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }


        public Task<JArray> GetMainListPaginado(string token, int limit, int pageNumber, int pageSize)
        {

            //string parameters = $"?00100010=Al*&00080020=20231001-20231231";
            string parameters = "";
            //00201206
            return DicomSendAsync(new APIRequest()
            {
                APIType = APIType.GET,
                Url = _validateUrl + "/packal/QidoRs/studies" + parameters,
                Token = token
            });
        }
    }


    // Pruebas realizadas:


    /*
        Construct the route with parameters pageNumber, pageSize, limit and orderBy
            string parameters = $"?limit={limit}&{orderBy}&page={pageNumber}&pagesize={pageSize}";
             Que tenga VELASCO en el nombre y se haya tomado entre el 2023/10/01 - 2023/12/31
            string variacion1 = "?patientname=VELASCO*&StudyDate=20231001-20231231&orderby=StudyDAte&limit=25"; // aprobada
             Lo mismo que el caso anterior pero usando tag en lugar de los nombres de los atributos
            string variacion2 = "?00100010=VELASCO*&00080020=20231001-20231231"; // aprobada
            // Que el nombre de la institucion comienze con HOSPITAL y el nombre de paciente tenga al
            string variacion3 = "?00080080=HOSPITAL*&00100010=al*"; // aprobada
            // Fecha de estudio mayor a 2023/01/01 y nombre tiene perez
            string variacion4 = "?00080020>20230101*&00100010=*perez*"; 
             Fecha de estudio mayor a 2023/01/01 y nombre tiene perez

            string variacion5 = "?includefield=00201206&includefield=00201208&00080020=%3E20230101&00100010=perez*";
            string variacion5 = "?includefield=00201206,00201208&00080020=%3E20230101&00100010=perez*";
     * 
     * */

}
