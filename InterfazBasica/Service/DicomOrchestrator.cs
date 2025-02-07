using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IDicomValidationService _validationService;
        private readonly IDicomDecompositionService _decompositionService;
        private readonly IDicomWebService _dicomWebService;
        //private int _institutionID;
        private readonly ILogger<DicomOrchestrator> _logger;

        public DicomOrchestrator(
            IDicomValidationService validationService,
            IDicomDecompositionService decompositionService,
            IDicomWebService dicomWebService,
            ILogger<DicomOrchestrator> logger)
        {
            _validationService = validationService;
            _decompositionService = decompositionService;
            _dicomWebService = dicomWebService;
            _logger = logger;
        }

        public string GetServerAEtitle()
        {
            return Aetitle;
        }

        public async Task<DicomStatus> StoreDicomData(DicomFile dicomFile)
        {
            try
            {
                // 1) Validar el archivo
                //var dicomStatus = _validationService.IsValidDicomFile(dicomFile);
                //if (dicomStatus != DicomStatus.Success)
                //{
                //    // Falló la validación, guardarlo en "Failed"
                //    await StoreDicomOnlocalDisk(dicomFile, StorageLocation.Failed);
                //    return dicomStatus;
                //}
                //// 2) Guardar en disco temporal (opcional)
                //await StoreDicomOnlocalDisk(dicomFile, StorageLocation.Temporary);
                //// 3) Enviar el DicomFile al canal
                //await DicomChannelSingleton.DicomChannel.Writer.WriteAsync(dicomFile);

                return DicomStatus.Success;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _logger.LogError(ex, "Error al recibir DICOM");
                return DicomStatus.ProcessingFailure;
            }
        }

        
    }
}
