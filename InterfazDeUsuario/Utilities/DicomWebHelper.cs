using InterfazDeUsuario.Models.Dtos.PacsDto;
using Newtonsoft.Json.Linq;

namespace InterfazDeUsuario.Utilities
{
    public static class DicomWebHelper
    {
        /// <summary>
        /// Converts a QIDO-RS JSON response to a list of StudyDto objects.
        /// </summary>
        /// <param name="qidoJson">The JArray representing the QIDO-RS JSON response.</param>
        /// <returns>A list of StudyDto objects containing the study metadata.</returns>
        public static List<StudyDto> ConvertQidoJsonToDtos(JArray qidoJson)
        {
            var studyDtos = new List<StudyDto>();

            foreach (var studyJson in qidoJson)
            {
                var studyDto = new StudyDto();

                // Study Instance UID
                if (studyJson["0020000D"] != null && studyJson["0020000D"]["Value"] != null)
                {
                    studyDto.StudyInstanceUID = studyJson["0020000D"]["Value"][0].ToString();
                }

                // Study Description
                if (studyJson["00081030"] != null && studyJson["00081030"]["Value"] != null)
                {
                    studyDto.StudyDescription = studyJson["00081030"]["Value"][0].ToString();
                }
                // Study Date
                if (studyJson["00080020"] != null && studyJson["00080020"]["Value"] != null)
                {
                    if (DateTime.TryParseExact(studyJson["00080020"]["Value"][0].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var studyDate))
                    {
                        studyDto.StudyDate = studyDate;
                    }
                }

                // Modality
                if (studyJson["00080060"] != null && studyJson["00080060"]["Value"] != null)
                {
                    studyDto.Modality = studyJson["00080060"]["Value"][0].ToString();
                }

                // Patient's Name
                if (studyJson["00100010"] != null && studyJson["00100010"]["Value"] != null && studyJson["00100010"]["Value"].HasValues)
                {
                    var patientNameObj = studyJson["00100010"]["Value"][0] as JObject;
                    if (patientNameObj != null && patientNameObj["Alphabetic"] != null)
                    {
                        studyDto.PatientName = patientNameObj["Alphabetic"].ToString();
                    }
                }

                // Patient's Sex
                if (studyJson["00100040"] != null && studyJson["00100040"]["Value"] != null)
                {
                    studyDto.PatientSex = studyJson["00100040"]["Value"][0].ToString();
                }

                // Patient's Age (if applicable)
                if (studyJson["00101010"] != null && studyJson["00101010"]["Value"] != null)
                {
                    studyDto.PatientAge = studyJson["00101010"]["Value"][0].ToString();
                }

                // Body Part Examined
                if (studyJson["00180015"] != null && studyJson["00180015"]["Value"] != null)
                {
                    studyDto.BodyPartExamined = studyJson["00180015"]["Value"][0].ToString();
                }

                studyDtos.Add(studyDto);
            }

            return studyDtos;
        }
    }
}
