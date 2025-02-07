using FellowOakDicom;
using FellowOakDicom.Serialization;
using InterfazBasica_DCStore.Service.BackgroundServices;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using Newtonsoft.Json;
using System.Data;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomDecompositionService2 : IDicomDecompositionService
    {
        // Log process (DicomChannelSingleton)
        private readonly ILogger<DicomChannel> _logger;

        public DicomDecompositionService2(ILogger<DicomChannel> logger)
        {
            _logger = logger;
        }

        public string ConvertDatasetToJson(DicomDataset dicomDataset)
        {
            try
            {
                var filteredDataset = new DicomDataset();

                foreach (var tag in DicomUtility.MetadataTags)
                {
                    if (dicomDataset.Contains(tag))
                    {
                        filteredDataset.Add(dicomDataset.GetDicomItem<DicomItem>(tag));
                    }
                }

                // Convertir a JSON usando fo-dicom 5.1.1
                return DicomJson.ConvertDicomToJson(
                    filteredDataset,
                    writeTagsAsKeywords: false, // Uso de Tags hexadecimal 
                    formatIndented: true       // JSON legible
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while reading from DicomFile channel.");
                return "";
            }


        }

       
    }
}
