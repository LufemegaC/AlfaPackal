

///// CONVERTIR DTO A DICOM+JSON

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


///// CONVERTIR DICOM+JSON A DTO


/// <summary>
        /// Converts a MainEntitiesCreateDto object to a DICOM JSON structure based on a predefined template.
        /// This method follows the DICOM STOW-RS standard to facilitate storing DICOM instances.
        /// Each tag in the DICOM JSON is documented with its corresponding meaning.
        /// </summary>
        /// <param name="dto">The DTO containing the metadata for the DICOM instance.</param>
        /// <returns>A JObject representing the DICOM JSON structure.</returns>
        public static JObject ConvertDtoToDicomJson(MetadataDto dto)
        {
            var dicomJson = new JObject();

            // Add fields only if they are valid (non-null, non-empty, or correctly formatted)

            // SOP Class UID
            if (!string.IsNullOrEmpty(dto.SOPClassUID))
            {
                dicomJson["00020010"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.SOPClassUID) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            // Referenced Series Sequence
            if (!string.IsNullOrEmpty(dto.SOPInstanceUID) && !string.IsNullOrEmpty(dto.SeriesInstanceUID))
            {
                dicomJson["00081199"] = new JObject
                {
                    ["vr"] = "SQ",
                    ["Value"] = new JArray(new JObject
                    {
                        // SOP Class UID
                        ["00080016"] = new JObject
                        {
                            ["vr"] = "UI",
                            ["Value"] = new JArray(dto.SOPInstanceUID) // Unique Identifier for the SOP Class
                        },
                        // SOP Instance UID
                        ["00080018"] = new JObject
                        {
                            ["vr"] = "UI",
                            ["Value"] = new JArray(dto.SeriesInstanceUID) // Unique Identifier for the SOP Instance
                        }
                    })
                };
            }

            // Series Description
            if (!string.IsNullOrEmpty(dto.SeriesDescription))
            {
                dicomJson["0008103E"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.SeriesDescription) // Description of the Series
                };
            }

            // Series Instance UID
            if (!string.IsNullOrEmpty(dto.SeriesInstanceUID))
            {
                dicomJson["0020000E"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.SeriesInstanceUID) // Unique Identifier for the Series
                };
            }

            // Series Number
            if (dto.SeriesNumber.HasValue)
            {
                dicomJson["00200010"] = new JObject
                {
                    ["vr"] = "IS",
                    ["Value"] = new JArray(dto.SeriesNumber.Value.ToString()) // Number that identifies the Series
                };
            }

            // Modality
            if (!string.IsNullOrEmpty(dto.Modality))
            {
                dicomJson["00080060"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.Modality) // Type of equipment that created the Series
                };
            }

            // Series Date
            if (dto.SeriesDateTime.HasValue && dto.SeriesDateTime.Value != DateTime.MinValue)
            {
                dicomJson["00080021"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.SeriesDateTime.Value.ToString("yyyyMMdd")) // Date the Series started
                };
            }

            // Patient Position
            if (!string.IsNullOrEmpty(dto.PatientPosition))
            {
                dicomJson["00180050"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.PatientPosition) // Position of the patient during the imaging
                };
            }

            // Study Description
            if (!string.IsNullOrEmpty(dto.StudyDescription))
            {
                dicomJson["00081030"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.StudyDescription) // Description of the Study
                };
            }

            // Study Date
            if (dto.StudyDate != DateTime.MinValue)
            {
                dicomJson["00080020"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.StudyDate.ToString("yyyyMMdd")) // Date the Study started
                };
            }

            // Study Time
            if (dto.StudyTime.HasValue)
            {
                var studyTimeValue = dto.StudyTime.Value;
                try
                {
                    var formattedStudyTime = new TimeSpan(studyTimeValue.Hours, studyTimeValue.Minutes, studyTimeValue.Seconds).ToString("hhmmss");
                    dicomJson["00080030"] = new JObject
                    {
                        ["vr"] = "TM",
                        ["Value"] = new JArray(formattedStudyTime) // Time the Study started
                    };
                }
                catch (FormatException)
                {
                    // Log or handle the incorrect format issue if needed
                }
            }



            // Accession Number
            if (!string.IsNullOrEmpty(dto.AccessionNumber))
            {
                dicomJson["00080050"] = new JObject
                {
                    ["vr"] = "SH",
                    ["Value"] = new JArray(dto.AccessionNumber) // Identifier for the Study
                };
            }

            // Institution Name
            if (!string.IsNullOrEmpty(dto.InstitutionName))
            {
                dicomJson["00080080"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.InstitutionName) // Name of the institution where the Study was performed
                };
            }

            // Patient's Name
            if (!string.IsNullOrEmpty(dto.PatientName))
            {
                dicomJson["00100010"] = new JObject
                {
                    ["vr"] = "PN",
                    ["Value"] = new JArray(dto.PatientName) // Full name of the patient
                };
            }

            // Patient's Sex
            if (!string.IsNullOrEmpty(dto.PatientSex))
            {
                dicomJson["00100040"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.PatientSex) // Sex of the patient
                };
            }

            // Patient's Birth Date
            if (dto.PatientBirthDate.HasValue && dto.PatientBirthDate.Value != DateTime.MinValue)
            {
                dicomJson["00100030"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.PatientBirthDate.Value.ToString("yyyyMMdd")) // Birth date of the patient
                };
            }

            // Study Instance UID
            if (!string.IsNullOrEmpty(dto.StudyInstanceUID))
            {
                dicomJson["0020000D"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.StudyInstanceUID) // Unique Identifier for the Study
                };
            }

            // Instance Number
            if (dto.InstanceNumber > 0)
            {
                dicomJson["00200011"] = new JObject
                {
                    ["vr"] = "IS",
                    ["Value"] = new JArray(dto.InstanceNumber.ToString()) // Number that identifies the Instance within the Series
                };
            }

            // Rows
            if (dto.Rows > 0)
            {
                dicomJson["00280010"] = new JObject
                {
                    ["vr"] = "US",
                    ["Value"] = new JArray(dto.Rows.ToString()) // Number of rows in the image
                };
            }

            // Columns
            if (dto.Columns > 0)
            {
                dicomJson["00280011"] = new JObject
                {
                    ["vr"] = "US",
                    ["Value"] = new JArray(dto.Columns.ToString()) // Number of columns in the image
                };
            }

            // Pixel Spacing
            if (!string.IsNullOrEmpty(dto.PixelSpacing))
            {
                dicomJson["00280030"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.PixelSpacing) // Physical distance between the centers of adjacent pixels
                };
            }

            // Image Position (Patient)
            if (!string.IsNullOrEmpty(dto.ImagePositionPatient))
            {
                dicomJson["00200032"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.ImagePositionPatient) // Position of the image in the patient
                };
            }

            // Image Orientation (Patient)
            if (!string.IsNullOrEmpty(dto.ImageOrientationPatient))
            {
                dicomJson["00200037"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.ImageOrientationPatient) // Orientation of the image in the patient
                };
            }

            // Body Part Examined
            if (!string.IsNullOrEmpty(dto.BodyPartExamined))
            {
                dicomJson["00082120"] = new JObject
                {
                    ["vr"] = "SH",
                    ["Value"] = new JArray(dto.BodyPartExamined) // Body part that was examined
                };
            }

            // Photometric Interpretation
            if (!string.IsNullOrEmpty(dto.PhotometricInterpretation))
            {
                dicomJson["00280004"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.PhotometricInterpretation) // Specifies the intended interpretation of the pixel data
                };
            }
            // -- Transaction -- //
            // Transaction UID
            if (!string.IsNullOrEmpty(dto.TransactionUID))
            {
                dicomJson["00081195"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.TransactionUID) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }
            // Transaction Status
            if (!string.IsNullOrEmpty(dto.TransactionStatus))
            {
                dicomJson["00080417"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.TransactionStatus) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            // Transaction Status (0008,0418) VR=LT VM=1
            if (!string.IsNullOrEmpty(dto.TransactionStatusComment))
            {
                dicomJson["00080418"] = new JObject
                {
                    ["vr"] = "LT",
                    ["Value"] = new JArray(dto.TransactionStatusComment) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            return dicomJson;
        }
    