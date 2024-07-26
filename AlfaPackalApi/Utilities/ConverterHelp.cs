using Api_PACsServer.Modelos.Dto;
using System.Net;

namespace Api_PACsServer.Utilities
{
    public class ConverterHelp
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
                IsExitoso = isSuccess,
                StatusCode = statusCode,
                Resultado = result,
                ErrorsMessages = errors ?? new List<string>()
            };
        }
    }
}
