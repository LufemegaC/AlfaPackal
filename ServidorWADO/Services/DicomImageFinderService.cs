using Microsoft.Extensions.Configuration;
using ServidorWADO.Services.IService;

namespace ServidorWADO.Services
{
    public class DicomImageFinderService : IDicomImageFinderService
    {
        private string _rootPath;


        public DicomImageFinderService(IConfiguration configuration)
        {
            _rootPath = configuration.GetValue<string>("DicomSettings:StoragePath");
        }

        public string GetImageByInstanceUid(string instanceUid)
        {
            throw new NotImplementedException();
        }
        
        public string GetImageByInstanceUid(string studyUid, string serieUid, string instanceUid)
        {
            // Construye la ruta completa usando studyUid y serieUid.
            var fullPath = Path.Combine(_rootPath, studyUid, serieUid);

            // Completa la ruta del archivo añadiendo instanceUid y la extensión .dcm.
            var filePath = Path.Combine(fullPath, instanceUid + ".dcm");

            // Verifica si el archivo existe, si no, lanza una excepción.
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("DICOM file not found", filePath);
            }

            return filePath;
        }
    }
}
