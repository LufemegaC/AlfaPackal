using Api_PACsServer.Services.IService.Dicom;
using FellowOakDicom;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Services
{
    public class DicomValidator : IDicomValidator
    {
        public ValidationResult ValidateAttributesBySOPClassUID(DicomDataset dataset)
        {
            var attributeNames = new List<string>();

            // Validar atributos comunes a todos los SOP Class UIDs
            ValidateCommonAttributes(dataset, attributeNames);

            // Obtener el SOP Class UID del dataset
            var sopClassUID = dataset.GetString(DicomTag.SOPClassUID);

            // Validar atributos específicos según el SOP Class UID
            switch (sopClassUID)
            {
                case var uid when uid == DicomUID.CTImageStorage.UID:
                    ValidateCTImageStorage(dataset, attributeNames);
                    break;

                case var uid when uid == DicomUID.MRImageStorage.UID:
                    ValidateMRImageStorage(dataset, attributeNames);
                    break;

                case var uid when uid == DicomUID.RTImageStorage.UID:
                    ValidateRTImageStorage(dataset, attributeNames);
                    break;

                case var uid when uid == DicomUID.UltrasoundImageStorage.UID:
                    ValidateUltrasoundImageStorage(dataset, attributeNames);
                    break;

                case var uid when uid == DicomUID.SecondaryCaptureImageStorage.UID:
                    ValidateSecondaryCaptureImageStorage(dataset, attributeNames);
                    break;

                default:
                    attributeNames.Add($"SOP Class UID not supported: {sopClassUID}");
                    break;
            }
            // Si no hay errores, retornamos Success
            if (!attributeNames.Any())
            {
                return ValidationResult.Success;
            }

            // Si hay errores de Type 1 (críticos), detener y retornar el error
            if (attributeNames.Any(a => a.Contains("Type 1")))
            {
                return new ValidationResult("Critical Type 1 error found.", attributeNames);
            }

            // Si solo hay advertencias de Type 2, continuamos con advertencias
            if (attributeNames.Any(a => a.Contains("Type 2")))
            {
                return new ValidationResult("Warnings found for Type 2 attributes.", attributeNames);
            }

            // Si hay otros errores, retornar un ValidationResult con los errores
            return new ValidationResult("Validation failed.", attributeNames);
        }

        private void ValidateCommonAttributes(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos comunes (Type 1 o 2) que deben estar presentes en todos los SOP Class UIDs

            // Type 1: Obligatorio, debe estar presente y contener un valor no vacío
            //ValidateType1(dataset, DicomTag.PatientID, memberNames);
            ValidateType1(dataset, DicomTag.StudyInstanceUID, memberNames);
            ValidateType1(dataset, DicomTag.SeriesInstanceUID, memberNames);
            ValidateType1(dataset, DicomTag.SOPInstanceUID, memberNames);
            ValidateType1(dataset, DicomTag.SOPClassUID, memberNames);

            // Type 2: Obligatorio, debe estar presente pero puede estar vacío
            ValidateType2(dataset, DicomTag.PatientID, memberNames);
            ValidateType2(dataset, DicomTag.PatientName, memberNames);
            ValidateType2(dataset, DicomTag.PatientBirthDate, memberNames);
            ValidateType2(dataset, DicomTag.PatientSex, memberNames);
            ValidateType2(dataset, DicomTag.StudyDate, memberNames);
            ValidateType2(dataset, DicomTag.StudyTime, memberNames);
            ValidateType2(dataset, DicomTag.Modality, memberNames);
            ValidateType2(dataset, DicomTag.Manufacturer, memberNames);
        }

        private void ValidateCTImageStorage(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos específicos para CT Image Storage

            // Type 1
            ValidateType1(dataset, DicomTag.ImagePositionPatient, memberNames);
            ValidateType1(dataset, DicomTag.ImageOrientationPatient, memberNames);
            ValidateType1(dataset, DicomTag.Rows, memberNames);
            ValidateType1(dataset, DicomTag.Columns, memberNames);
            ValidateType1(dataset, DicomTag.PixelSpacing, memberNames);
            ValidateType1(dataset, DicomTag.BitsAllocated, memberNames);
            ValidateType1(dataset, DicomTag.BitsStored, memberNames);
            ValidateType1(dataset, DicomTag.HighBit, memberNames);
            ValidateType1(dataset, DicomTag.PixelRepresentation, memberNames);
            ValidateType1(dataset, DicomTag.PixelData, memberNames);

            // Type 2
            ValidateType2(dataset, DicomTag.KVP, memberNames);
            ValidateType2(dataset, DicomTag.SliceThickness, memberNames);
        }

        private void ValidateMRImageStorage(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos específicos para MR Image Storage

            // Type 1
            ValidateType1(dataset, DicomTag.ImagePositionPatient, memberNames);
            ValidateType1(dataset, DicomTag.ImageOrientationPatient, memberNames);
            ValidateType1(dataset, DicomTag.Rows, memberNames);
            ValidateType1(dataset, DicomTag.Columns, memberNames);
            ValidateType1(dataset, DicomTag.PixelSpacing, memberNames);
            ValidateType1(dataset, DicomTag.BitsAllocated, memberNames);
            ValidateType1(dataset, DicomTag.BitsStored, memberNames);
            ValidateType1(dataset, DicomTag.HighBit, memberNames);
            ValidateType1(dataset, DicomTag.PixelRepresentation, memberNames);
            ValidateType1(dataset, DicomTag.PixelData, memberNames);

            // Type 2
            ValidateType2(dataset, DicomTag.RepetitionTime, memberNames);
            ValidateType2(dataset, DicomTag.EchoTime, memberNames);
        }

        private void ValidateRTImageStorage(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos específicos para RT Image Storage

            // Type 1
            ValidateType1(dataset, DicomTag.ImageType, memberNames);
            ValidateType1(dataset, DicomTag.SamplesPerPixel, memberNames);
            ValidateType1(dataset, DicomTag.PhotometricInterpretation, memberNames);
            ValidateType1(dataset, DicomTag.Rows, memberNames);
            ValidateType1(dataset, DicomTag.Columns, memberNames);
            ValidateType1(dataset, DicomTag.BitsAllocated, memberNames);
            ValidateType1(dataset, DicomTag.BitsStored, memberNames);
            ValidateType1(dataset, DicomTag.HighBit, memberNames);
            ValidateType1(dataset, DicomTag.PixelRepresentation, memberNames);
            ValidateType1(dataset, DicomTag.PixelData, memberNames);

            // Type 2
            ValidateType2(dataset, DicomTag.RTImageLabel, memberNames);
            ValidateType2(dataset, DicomTag.RTImagePlane, memberNames);
        }

        private void ValidateUltrasoundImageStorage(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos específicos para Ultrasound Image Storage

            // Type 1
            ValidateType1(dataset, DicomTag.SamplesPerPixel, memberNames);
            ValidateType1(dataset, DicomTag.PhotometricInterpretation, memberNames);
            ValidateType1(dataset, DicomTag.Rows, memberNames);
            ValidateType1(dataset, DicomTag.Columns, memberNames);
            ValidateType1(dataset, DicomTag.BitsAllocated, memberNames);
            ValidateType1(dataset, DicomTag.BitsStored, memberNames);
            ValidateType1(dataset, DicomTag.HighBit, memberNames);
            ValidateType1(dataset, DicomTag.PixelRepresentation, memberNames);
            ValidateType1(dataset, DicomTag.PixelData, memberNames);

            // Type 2
            ValidateType2(dataset, DicomTag.PresentationIntentType, memberNames);
            ValidateType2(dataset, DicomTag.PresentationLUTShape, memberNames);
        }

        // Repite para los demás SOP Class UIDs...

        private void ValidateSecondaryCaptureImageStorage(DicomDataset dataset, List<string> memberNames)
        {
            // Atributos específicos para Secondary Capture Image Storage

            // Type 1
            ValidateType1(dataset, DicomTag.SamplesPerPixel, memberNames);
            ValidateType1(dataset, DicomTag.PhotometricInterpretation, memberNames);
            ValidateType1(dataset, DicomTag.Rows, memberNames);
            ValidateType1(dataset, DicomTag.Columns, memberNames);
            ValidateType1(dataset, DicomTag.BitsAllocated, memberNames);
            ValidateType1(dataset, DicomTag.BitsStored, memberNames);
            ValidateType1(dataset, DicomTag.HighBit, memberNames);
            ValidateType1(dataset, DicomTag.PixelRepresentation, memberNames);
            ValidateType1(dataset, DicomTag.PixelData, memberNames);
        }

        // Agrega métodos similares para los SOP Class UIDs restantes

        // Métodos auxiliares para validar tipos

        private void ValidateType1(DicomDataset dataset, DicomTag tag, List<string> memberNames)
        {
            if (!dataset.Contains(tag) || dataset.GetValueCount(tag) == 0)
            {
                memberNames.Add($"Atributo Type 1 faltante o vacío: {tag.DictionaryEntry.Name} ({tag})");
            }
        }

        private void ValidateType2(DicomDataset dataset, DicomTag tag, List<string> memberNames)
        {
            if (!dataset.Contains(tag))
            {
                memberNames.Add($"Atributo Type 2 faltante: {tag.DictionaryEntry.Name} ({tag})");
            }
            // El valor puede estar vacío, no se valida si está vacío
        }
    }
}
