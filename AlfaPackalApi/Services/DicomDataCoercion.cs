using Api_PACsServer.Services.IService.Dicom;
using FellowOakDicom;

namespace Api_PACsServer.Services
{
    public class DicomDataCoercion : IDicomDataCoercion
    {
        public void Anonymize(DicomDataset dataset)
        {
            // Lista de DicomTags que contienen información de identificación personal
            var tagsToRemove = new List<DicomTag>
            {
                DicomTag.PatientName,
                DicomTag.PatientID,
                DicomTag.PatientBirthDate,
                DicomTag.PatientSex,
                DicomTag.PatientAddress,
                DicomTag.PatientTelephoneNumbers,
                DicomTag.PatientWeight,
                DicomTag.PatientSize,
                //DicomTag.MedicalRecordLocator,RETIRED
                DicomTag.InstitutionName,
                DicomTag.InstitutionAddress,
                DicomTag.ReferringPhysicianName,
                DicomTag.StudyID,
                DicomTag.AccessionNumber,
                DicomTag.StudyDescription,
                DicomTag.SeriesDescription,
                DicomTag.PerformingPhysicianName,
                DicomTag.ProtocolName,
                DicomTag.OperatorsName,
                DicomTag.PatientBirthTime,
                DicomTag.PatientComments,
                DicomTag.DeviceSerialNumber,
                DicomTag.SoftwareVersions,
                DicomTag.ScheduledProcedureStepSequence,
                DicomTag.RequestAttributesSequence,
                // Agrega más tags según el estándar DICOM y tus necesidades
            };

            // Remover los tags de identificación personal
            foreach (var tag in tagsToRemove)
            {
                dataset.Remove(tag);
            }

            // Asignar valores genéricos o anónimos a algunos campos si es necesario
            dataset.AddOrUpdate(DicomTag.PatientName, "Anonymized");
            dataset.AddOrUpdate(DicomTag.PatientID, "00000000");
            dataset.AddOrUpdate(DicomTag.PatientBirthDate, "19000101");
            dataset.AddOrUpdate(DicomTag.PatientSex, "O"); // Other/Unknown
        }

        public void CorrectData(DicomDataset dataset)
        {
            // Normalizar el nombre del paciente
            if (dataset.Contains(DicomTag.PatientName))
            {
                var patientName = dataset.GetString(DicomTag.PatientName);
                var normalizedPatientName = NormalizeName(patientName);
                dataset.AddOrUpdate(DicomTag.PatientName, normalizedPatientName);
            }

            // Corregir el formato de la fecha de nacimiento
            if (dataset.Contains(DicomTag.PatientBirthDate))
            {
                var birthDate = dataset.GetString(DicomTag.PatientBirthDate);
                var correctedBirthDate = CorrectDateFormat(birthDate);
                dataset.AddOrUpdate(DicomTag.PatientBirthDate, correctedBirthDate);
            }

            // Validar y corregir el PatientID si es necesario
            if (dataset.Contains(DicomTag.PatientID))
            {
                var patientID = dataset.GetString(DicomTag.PatientID);
                if (!IsValidPatientID(patientID))
                {
                    var correctedPatientID = GenerateValidPatientID();
                    dataset.AddOrUpdate(DicomTag.PatientID, correctedPatientID);
                }
            }

            // Validar y corregir el sexo del paciente
            if (dataset.Contains(DicomTag.PatientSex))
            {
                var patientSex = dataset.GetString(DicomTag.PatientSex);
                if (!IsValidPatientSex(patientSex))
                {
                    dataset.AddOrUpdate(DicomTag.PatientSex, "O"); // Other/Unknown
                }
            }

            // Asegurar que el Modality está presente y es válido
            if (dataset.Contains(DicomTag.Modality))
            {
                var modality = dataset.GetString(DicomTag.Modality);
                if (!IsValidModality(modality))
                {
                    dataset.AddOrUpdate(DicomTag.Modality, "OT"); // Other
                }
            }
            else
            {
                // Si no está presente, asignar un valor por defecto
                dataset.AddOrUpdate(DicomTag.Modality, "OT"); // Other
            }

            // Verificar que los UIDs están en el formato correcto
            CorrectUIDFormat(dataset, DicomTag.StudyInstanceUID);
            CorrectUIDFormat(dataset, DicomTag.SeriesInstanceUID);
            CorrectUIDFormat(dataset, DicomTag.SOPInstanceUID);

            // Asegurar que los atributos de imagen son consistentes
            if (dataset.Contains(DicomTag.Rows) && dataset.Contains(DicomTag.Columns))
            {
                var rows = dataset.GetSingleValueOrDefault<int>(DicomTag.Rows, 0);
                var columns = dataset.GetSingleValueOrDefault<int>(DicomTag.Columns, 0);
                if (rows <= 0 || columns <= 0)
                {
                    // Asignar valores por defecto si los valores no son válidos
                    dataset.AddOrUpdate(DicomTag.Rows, 512);
                    dataset.AddOrUpdate(DicomTag.Columns, 512);
                }
            }
            else
            {
                // Asignar valores por defecto si los tags no están presentes
                dataset.AddOrUpdate(DicomTag.Rows, 512);
                dataset.AddOrUpdate(DicomTag.Columns, 512);
            }
        }

        // Métodos auxiliares
        private string NormalizeName(string name)
        {
            // Convertir a mayúsculas y eliminar espacios adicionales
            return name?.Trim().ToUpperInvariant();
        }

        private string CorrectDateFormat(string date)
        {
            // El formato DICOM para fechas es YYYYMMDD
            if (DateTime.TryParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate.ToString("yyyyMMdd");
            }
            else if (DateTime.TryParse(date, out parsedDate))
            {
                return parsedDate.ToString("yyyyMMdd");
            }
            else
            {
                // Si no se puede parsear, asignar una fecha por defecto
                return "19000101";
            }
        }

        private bool IsValidPatientID(string patientID)
        {
            // Validar que el PatientID no esté vacío y cumpla con el formato deseado
            return !string.IsNullOrWhiteSpace(patientID) && patientID.Length <= 64;
        }

        private string GenerateValidPatientID()
        {
            // Generar un PatientID único
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

        private bool IsValidPatientSex(string patientSex)
        {
            // Los valores permitidos son "M", "F", "O"
            return patientSex == "M" || patientSex == "F" || patientSex == "O";
        }

        private bool IsValidModality(string modality)
        {
            // Lista de modalidades DICOM válidas (no exhaustiva)
            var validModalities = new HashSet<string>
            {
                "CR", "CT", "MR", "US", "NM", "OT", "BI", "XA", "RF", "MG", "PT", "DX", "SC", "XC"
            };
            return validModalities.Contains(modality);
        }
        private void CorrectUIDFormat(DicomDataset dataset, DicomTag tag)
        {
            if (dataset.Contains(tag))
            {
                var uid = dataset.GetString(tag);
                if (!DicomUID.IsValidUid(uid))
                {
                    // Generar un nuevo UID válido
                    var newUid = DicomUIDGenerator.GenerateDerivedFromUUID().UID;
                    dataset.AddOrUpdate(tag, newUid);
                }
            }
            else
            {
                // Generar y asignar un nuevo UID
                var newUid = DicomUIDGenerator.GenerateDerivedFromUUID().UID;
                dataset.AddOrUpdate(tag, newUid);
            }
        }
    }
}
