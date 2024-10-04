using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using System.Net;

namespace Api_PACsServer.Utilities
{
    public static class ConverterHelp
    {
        public static string GetDescModality(string modality)
        {
            return modality switch
            {
                "CT" => "Tomografía Computarizada",
                "CR" => "Radiografía Computarizada",
                "MR" => "Resonancia Magnética",
                "US" => "Ultrasonido",
                "XR" => "Radiografía",
                "NM" => "Medicina Nuclear",
                "DX" => "Radiografía Digital",
                "MG" => "Mamografía",
                "IO" => "Imagen Intraoral",
                _ => "Otro"
            };
        }

        public static string GetDescBodyPart(string bodyPart)
        {
            return bodyPart switch
            {
                "HEAD" => "Cabeza",
                "NECK" => "Cuello",
                "CHEST" => "Tórax",
                "ABDOMEN" => "Abdomen",
                "PELVIS" => "Pelvis",
                "SPINE" => "Columna vertebral",
                "SHOULDER" => "Hombro",
                "ELBOW" => "Codo",
                "WRIST" => "Muñeca",
                "HAND" => "Mano",
                "HIP" => "Cadera",
                "KNEE" => "Rodilla",
                "ANKLE" => "Tobillo",
                "FOOT" => "Pie",
                "HEART" => "Corazón",
                "LUNG" => "Pulmón",
                "LIVER" => "Hígado",
                "KIDNEY" => "Riñón",
                "BRAIN" => "Cerebro",
                _ => "S/V"
            };
        }

        public static APIResponse CreateResponse(bool isSuccess, HttpStatusCode statusCode, object result = null, List<string> errors = null)
        {
            return new APIResponse
            {
                IsSuccessful = isSuccess,
                StatusCode = statusCode,
                ResponseData = result,
                ErrorsMessages = errors ?? new List<string>()
            };
        }

        public static APIResponse ConvertToApiResponse(OperationResult operationResult)
        {
            var apiResponse = new APIResponse();

            if (operationResult.IsSuccess)
            {
                if (string.IsNullOrEmpty(operationResult.ErrorMessage))
                {
                    apiResponse.StatusCode = HttpStatusCode.OK;
                    apiResponse.IsSuccessful = true;
                    apiResponse.ResponseData = operationResult.ResponseData; // O lo que corresponda
                }
                else
                {
                    apiResponse.StatusCode = HttpStatusCode.Accepted;
                    apiResponse.IsSuccessful = false;
                    apiResponse.ResponseData = operationResult.ResponseData; // O lo que corresponda
                }  
            }
            else
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.IsSuccessful = false;
                apiResponse.ErrorsMessages = new List<string> { operationResult.ErrorMessage };
            }

            return apiResponse;
        }
    }
}
