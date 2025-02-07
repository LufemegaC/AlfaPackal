using Api_PACsServer.Factories;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb;
using FellowOakDicom;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Api_PACsServer.Utilities.QueryOperators;
using System.Reflection;
using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using FellowOakDicom.Network;
using Api_PACsServer.Models.Attributes;
using System.Globalization;
using FellowOakDicom.Serialization;
using System.Text.RegularExpressions;

namespace Api_PACsServer.Utilities
{
    public class DicomWebHelper
    {
        /// *** STOW-RS SECCION *** ///
        
        public static async Task<StowRsValidationResult> ValidateRequest(HttpRequest request/*, ILogger logger*/)
        {
            var validationResult = new StowRsValidationResult();

            // ESTA SECCION SE DEBE PASAR AL SERVICIO DEL ORCHESTRATOR

            //// 1. Validación de IP de origen
            //var remoteIp = request.HttpContext.Connection.RemoteIpAddress;
            //if (!IsLocalDicomServer(remoteIp))
            //{
            //    logger.LogWarning($"Intento de acceso no autorizado desde IP: {remoteIp}");
            //    validationResult.IsValid = false;
            //    validationResult.ErrorMessage = "Solicitud rechazada: Origen no permitido";
            //    return validationResult;
            //}

            // Check if the request content type is multipart/related
            if (!request.ContentType.Contains("multipart/related", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Request does not contain multipart/related content.");

            var contentType = request.ContentType;
            var boundary = GetBoundaryFromContentType(contentType);

            if (!IsValidBoundary(boundary))
            {
                //logger.LogWarning($"Boundary no válido detectado: {boundary}");
                validationResult.IsValid = false;
                validationResult.ErrorMessage = "Formato de boundary no válido";
                return validationResult;
            }

            // 3. Validación de estructura multipart
            if (!ValidateMultipartStructure(request))
            {
                validationResult.IsValid = false;
                validationResult.ErrorMessage = "Estructura DICOM no válida en el payload";
                return validationResult;
            }

            validationResult.IsValid = true;
            return validationResult;
        }

       

        private static readonly HashSet<string> AllowedContentTypes = new()
        {
            "application/dicom+json",
            "application/dicom"
        };

        private static bool IsValidBoundary(string boundary)
        {
            if (string.IsNullOrEmpty(boundary)) return false;

            // Validar formato: PACKAL_ + UTC timestamp (14 dígitos)
            var pattern = @"^PACKAL_\d{14}$";
            return Regex.IsMatch(boundary, pattern);
        }

        private static bool ValidateMultipartStructure(HttpRequest request)
        {
            try
            {
                var reader = new MultipartReader(GetBoundaryFromContentType(request.ContentType), request.Body);
                MultipartSection section;
                bool hasMetadata = false, hasInstance = false;

                while ((section = reader.ReadNextSectionAsync().Result) != null)
                {
                    var contentType = section.ContentType?.Split(';')[0].Trim();

                    if (!AllowedContentTypes.Contains(contentType))
                    {
                        return false;
                    }

                    // Validar alternancia de metadata/instancia
                    if (contentType == "application/dicom+json") hasMetadata = true;
                    if (contentType == "application/dicom") hasInstance = true;
                }

                // Debe tener al menos un par metadata/instancia
                return hasMetadata && hasInstance;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts the boundary from the Content-Type header of the request.
        /// </summary>
        /// <param name="contentType">The Content-Type header value.</param>
        /// <returns>The boundary string if found; otherwise, null.</returns>
        public static string GetBoundaryFromContentType(string contentType)
        {
            var elements = contentType.Split(';');
            foreach (var element in elements)
            {
                var trimmedElement = element.Trim();
                if (trimmedElement.StartsWith("boundary="))
                {
                    return trimmedElement.Substring("boundary=".Length).Trim('"');
                }
            }
            return null;
        }
        /// *** STOW-RS SECCION : ENDS *** ///

        /// *** QIDO-RS SECCION : BEGINS *** ///

        public static QueryRequestParameters<T> TranslateRequestParameters<T> (Dictionary<string, List<string>> queryParams) where T : BaseDicomQueryParametersDto, new ()
        {
            var additionalParams = new AdditionalParameters();
            // Create an instance of the DTO type specified by T
            var dicomParamsDto = new T();
            var dicomLevel = dicomParamsDto.DicomQueryLevel;
            foreach (var param in queryParams)
            {
                // Iterar a través de cada valor de la lista para la clave dada
                foreach (var value in param.Value)
                {
                    // Decodificar el valor del parámetro
                    var decodedValue = System.Web.HttpUtility.UrlDecode(value);
                    string columnName;

                    if (param.Key.Equals("includefield", StringComparison.OrdinalIgnoreCase))
                    {
                        // Procesar IncludeFields, soportando tanto una sola declaración con múltiples valores
                        // como múltiples declaraciones con un solo valor cada una.
                        var fields = decodedValue.Split(',');
                        foreach (var field in fields)
                        {
                            string includeFieldName;
                            if (IsNumericTag(field))
                            {
                                // Convertir el string hexadecimal al DicomTag correspondiente
                                var dicomTag = DicomTag.Parse(field);
                                if (dicomTag != null)
                                {
                                    // Obtener el nombre del tag directamente desde el DicomTag
                                    includeFieldName = dicomTag.DictionaryEntry.Keyword;
                                }
                                else
                                {
                                    throw new InvalidOperationException($"DicomTag no válido para el valor: {field}");
                                }
                            }
                            else
                            {
                                // Validar si el nombre del atributo es un DICOM Tag válido
                                var dicomTag = DicomDictionary.Default.FirstOrDefault(tag => tag.Keyword.Equals(field, StringComparison.OrdinalIgnoreCase));
                                if (dicomTag != null)
                                {
                                    includeFieldName = dicomTag.Keyword;
                                }
                                else
                                {
                                    throw new InvalidOperationException($"Atributo DICOM no válido: {field}");
                                }
                            }
                            // Validar si el campo existe en el modelo Study
                            if (!ValidatePropertyExists(includeFieldName, dicomLevel))
                            {
                                throw new InvalidOperationException($"The field '{includeFieldName}' do not exists in {dicomLevel} level.");
                            }
                            // Agregar el campo validado a la lista de IncludeFields en el DTO
                            dicomParamsDto.IncludeFields.Add(includeFieldName);
                        }
                    }
                    else
                    {
                        // Determinar si el parámetro es un nombre de atributo o un tag DICOM
                        if (IsNumericTag(param.Key))
                        {
                            // Convertir el string hexadecimal al DicomTag correspondiente
                            var dicomTag = DicomTag.Parse(param.Key);
                            if (dicomTag != null)
                            {
                                // Obtener el nombre del tag directamente desde el DicomTag
                                columnName = dicomTag.DictionaryEntry.Keyword;
                            }
                            else
                            {
                                throw new InvalidOperationException($"DicomTag no válido para el valor: {param.Key}");
                            }
                        }
                        else
                        {
                            columnName = param.Key;
                        }

                        // Crear el QueryParameter correspondiente
                        var queryParameter = QueryParameterFactory.CreateQueryParameter(columnName, decodedValue);

                        // Asignar el QueryParameter al DTO correspondiente
                        if (queryParameter.Category == QueryParameterCategory.Additional)
                        {
                            AssignControlParameter(additionalParams, columnName, queryParameter);
                        }
                        else if (queryParameter.Category == QueryParameterCategory.Dicom)
                        {
                            AssignGenericParameter(dicomParamsDto, columnName, queryParameter);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Unrecognized parameter category for {param.Key}");
                        }
                    }
                }
            }
            dicomParamsDto.Validateparameters();
            return new QueryRequestParameters<T>(additionalParams, dicomParamsDto);
        }

        public static bool ValidatePropertyExists(string propertyName, DicomQueryRetrieveLevel level)
        {
            // Obtener el tipo del modelo MetadataDto
            var metadataType = typeof(MetadataDto);

            // Buscar propiedades cuyo nombre coincida con propertyName
            var matchingProperty = metadataType.GetProperties()
                .FirstOrDefault(prop =>
                    prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) &&
                    prop.GetCustomAttributes(typeof(DicomLevelAttribute), true)
                        .OfType<DicomLevelAttribute>()
                        .Any(attr => attr.Level == level)
                );

            // Retorna true si encontró una propiedad que cumple las condiciones, false si no
            return matchingProperty != null;
        }




        private static void AssignControlParameter(AdditionalParameters controlParamsDto, string paramKey, QueryParameter queryParameter)
        {
            switch (paramKey.ToLower())
            {
                case "limit":
                    controlParamsDto.Limit = queryParameter;
                    break;
                case "orderby":
                    controlParamsDto.OrderBy = queryParameter;
                    break;
                case "offset":
                    controlParamsDto.Offset = queryParameter;
                    break;
                //case "includefields":
                //    controlParamsDto.IncludeFields.Add(queryParameter.Value);
                    break;
                case "format":
                    controlParamsDto.Format = queryParameter;
                    break;
                case "page":
                    controlParamsDto.Page = queryParameter;
                    break;
                case "pagesize":
                    controlParamsDto.PageSize = queryParameter;
                    break;
                default:
                    throw new InvalidOperationException($"Parámetro de control {paramKey} no reconocido.");
            }
        }

        // Method to assign a study parameter to StudyQueryParametersDto

        private static void AssignGenericParameter<T>(T dto, string key, QueryParameter queryParameter) where T : BaseDicomQueryParametersDto
        {
            var property = typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (property != null && property.PropertyType == typeof(QueryParameter))
            {
                // Set the property value in the DTO of type T
                property.SetValue(dto, queryParameter);
            }
        }

        // Método auxiliar para determinar si el nombre de la columna es un valor numérico de un DICOM Tag
        private static bool IsNumericTag(string columnName)
        {
            return ulong.TryParse(columnName, System.Globalization.NumberStyles.HexNumber, null, out _);
        }


        /// <summary>
        /// Verifica si un campo existe en el modelo Study.
        /// </summary>
        /// <param name="fieldName">El nombre del campo a verificar.</param>
        /// <returns>Verdadero si el campo existe, falso en caso contrario.</returns>
        private static bool IsFieldValidForStudy(string fieldName)
        {
            var studyProperties = typeof(Study).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return studyProperties.Any(prop => prop.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Converts a list of DICOM DTOs (StudyDto, SerieDto, or InstanceDto) to a JSON+DICOM format.
        /// </summary>
        /// <typeparam name="T">The type of DTO being converted (StudyDto, SerieDto, or InstanceDto).</typeparam>
        /// <param name="dicomDtos">The list of DTOs to be converted.</param>
        /// <returns>A JSON string representing the DICOM data in JSON+DICOM format.</returns>
        public static string ConvertDicomDtosToDicomJsonString<T>(List<T> dicomDtos)
        {
            if (dicomDtos == null || !dicomDtos.Any())
                throw new ArgumentException("The input list is null or empty.");

            var dicomJsonArray = new JArray();

            foreach (var dto in dicomDtos)
            {
                var dicomJson = new JObject();

                // Use reflection to iterate through the properties of the DTO
                foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = property.GetValue(dto);

                    if (value != null) // Check if the property has a value
                    {
                        // Attempt to get the corresponding DICOM Tag
                        var dicomTag = GetDicomTagFromPropertyName(property.Name);

                        if (dicomTag != null)
                        {
                            // Convert the value to the appropriate DICOM JSON format
                            var formattedValue = FormatDicomJsonValue(dicomTag, value);
                            dicomJson[dicomTag.ToString()] = formattedValue;
                        }
                    }
                }

                dicomJsonArray.Add(dicomJson);
            }

            return dicomJsonArray.ToString();
        }

        /// <summary>
        /// Gets the corresponding DICOM Tag for a given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The corresponding DICOM Tag or null if not found.</returns>
        private static DicomTag? GetDicomTagFromPropertyName(string propertyName)
        {
            // Buscar en el DicomDictionary.Default que contiene todos los tags DICOM estándar.
            var entry = DicomDictionary.Default.FirstOrDefault(e =>
                e.Keyword.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            // Verifica si el tag fue encontrado y retorna el DicomTag correspondiente
            return entry?.Tag;
        } 

        /// <summary>
        /// Formats a property value into the appropriate DICOM JSON format.
        /// </summary>
        /// <param name="dicomTag">The DICOM Tag associated with the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>A JObject representing the formatted DICOM JSON entry.</returns>
        private static JObject FormatDicomJsonValue(DicomTag dicomTag, object value)
        {

            // Obtiene las representaciones de valor (Value Representations) asociadas al DicomTag
            var vrs = dicomTag.DictionaryEntry.ValueRepresentations;

            // Usa el primer VR si hay más de uno
            string vr = vrs.Length > 0 ? vrs[0].Code : "UN"; // "UN" representa Unknown VR como fallback

            // Crear el array JSON basado en el valor proporcionado
            JArray jsonValue;
            if (value is IEnumerable<string> stringValues)
            {
                jsonValue = new JArray(stringValues);
            }
            else if (value is IEnumerable<object> objectValues)
            {
                jsonValue = new JArray(objectValues.Select(o => o.ToString()));
            }
            else
            {
                jsonValue = new JArray(value.ToString());
            }

            // Crear el objeto JSON que representa el formato DICOM JSON
            return new JObject
            {
                ["vr"] = vr,
                ["Value"] = jsonValue
            };
        }


        /// *** QIDO-RS SECCION : ENDS *** ///

        // --- CONVERTER ZONE --- //
        private static readonly HashSet<string> SupportedSopClasses = new HashSet<string>
        {
            DicomUID.CTImageStorage.UID,                         // Tomografía Computarizada
            DicomUID.MRImageStorage.UID,                         // Resonancia Magnética
            DicomUID.RTImageStorage.UID,
            DicomUID.UltrasoundImageStorage.UID,                 // Ultrasonido
            DicomUID.SecondaryCaptureImageStorage.UID,           // Captura Secundaria
            DicomUID.DigitalXRayImageStorageForPresentation.UID, // Radiografía Digital para Presentación
            DicomUID.DigitalMammographyXRayImageStorageForPresentation.UID, // Mamografía Digital para Presentación
            DicomUID.NuclearMedicineImageStorage.UID,            // Medicina Nuclear
            DicomUID.EnhancedCTImageStorage.UID,                 // Imágenes CT Mejoradas
            DicomUID.EnhancedMRImageStorage.UID,                 // Imágenes MR Mejoradas
            DicomUID.EnhancedUSVolumeStorage.UID                // Volumen de Ultrasonido Mejorado
        };

        private static readonly HashSet<string> SupportedTransferSyntaxes = new HashSet<string>
        {
            DicomUID.ExplicitVRLittleEndian.UID,
            //DicomUID.ExplicitVRBigEndian.UID, //RETIRED
            DicomUID.ImplicitVRLittleEndian.UID,
            // Add other supported Transfer Syntaxes
        };


        



        // PENDIENTES DEFINIR SU POSICION :

        

    }
}
