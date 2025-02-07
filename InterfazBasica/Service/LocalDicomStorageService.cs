using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.IService;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
{
    public class LocalDicomStorageService : ILocalDicomStorageService
    {

        public async Task<DicomStatus> StoreDicomFileAsync(DicomFile dicomFile, string rootPath)
        {
            string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
            string studyUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, null);
            string seriesUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, null);
            string tempFilePath = PathFileDicomConstructor(studyUID, seriesUID, instanceUID, rootPath);
            // Guarda el archivo DICOM en la ruta especificada.
            await dicomFile.SaveAsync(tempFilePath);
            // Retorna una respuesta de éxito.
            return DicomStatus.Success;
        }


        private static string PathFileDicomConstructor(string studyUID, string seriesUID, string instanceUID, string basePath)
        {
            string fullPath = Path.Combine(basePath, studyUID, seriesUID);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return Path.Combine(fullPath, instanceUID + ".dcm");
        }
    }
}
