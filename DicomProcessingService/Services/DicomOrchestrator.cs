using AutoMapper;
using DicomProcessingService.Services.DicomServer.DServices;
using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Network;

namespace DicomProcessingService.Services
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IDicomValidationService _validationService;
        private readonly IDicomDecompositionService _decompositionService;
        private IEstudioService _estudioService;

        public DicomOrchestrator(
            IDicomValidationService validationService,
            IDicomDecompositionService decompositionService,
            IEstudioService estudioService)
        {
            _validationService = validationService;
            _decompositionService = decompositionService;
            _estudioService = estudioService;
            // Suscribirse al evento AssociationReceived de EchoService
            //EchoService.AssociationReceived += HandleCStoreRequestAsync;
        }

        public Task HandleCStoreRequestAsync(DicomCStoreRequest cStoreRequest)
        {

            // UIDs           
            // Validaciones 
            var studyUid = cStoreRequest.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID).Trim();
            var instUid = cStoreRequest.SOPInstanceUID.UID;
            var seriesUid = cStoreRequest.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID).Trim();
            var sopClassUID = cStoreRequest.Dataset.GetSingleValue<string>(DicomTag.SOPClassUID);
            throw new NotImplementedException();
        }

        private void HandleCStoreRequestAsync(object sender, DicomAssociation e)
        {
            //HandleCStoreRequestAsync
            //// Begin by validating the DICOM file
            //if (!_validationService.ValidateAgainstStandard())
            //{
            //    // Handle validation failure
            //    throw new InvalidOperationException("DICOM file validation failed.");
            //}




        }

        // Placeholder for the registration methods that would be implemented
        // to interact with the database or other services.
        private async Task RegisterStudyAsync(Dictionary<DicomTag, object> metadata)
        {
            // Logic to register the study
        }

        //private async Task RegisterSeriesAsync(Dictionary<DicomTag, object> metadata)
        //{
        //    // Logic to register the series
        //}

        //private async Task RegisterImagesAsync(IEnumerable<byte[]> imageFrames)
        //{
        //    // Logic to register the images
        //}
    }
}
