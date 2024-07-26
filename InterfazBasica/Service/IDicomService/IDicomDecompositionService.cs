using FellowOakDicom;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Models;

namespace InterfazBasica_DCStore.Service.IDicomService
{
    public interface IDicomDecompositionService
    {
        // ** Metodos generales
        Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset);
        IEnumerable<byte[]> ExtractImageFrames(DicomFile dicomFile);
        string GetDicomElementAsString(DicomDataset dicomDataset, DicomTag dicomTag);
        DateTime? GetStudyDateTime(DicomDataset dicomDataset);
        void ConvertDicomToFile(DicomFile dicomFile, string outputPath, DicomTransferSyntax transferSyntax);
        //void UpdatePatientData(DicomFile dicomFile, string patientName, string patientId);
        void AnonymizeDicomFile(DicomFile dicomFile);

        //Get file size from Dicom in MB
        double GetFileSizeInMB(DicomFile dicomFile);

        /////*** Entidades 
        //PacienteCreateDto DecomposeDicomToPaciente(Dictionary<DicomTag, object> metadata);
        //EstudioCreateDto DecomposeDicomToEstudio(Dictionary<DicomTag, object> metadata);
        //SerieCreateDto DecomposeDicomToSerie(Dictionary<DicomTag, object> metadata);
        //ImagenCreateDto DecomposeDicomToImagen(Dictionary<DicomTag, object> metadata);

        ///*** Entidades 
        Task<PacienteCreateDto> DecomposeDicomToPaciente(Dictionary<DicomTag, object> metadata);
        Task<EstudioCreateDto> DecomposeDicomToEstudio(Dictionary<DicomTag, object> metadata);
        Task<SerieCreateDto> DecomposeDicomToSerie(Dictionary<DicomTag, object> metadata);
        Task<ImagenCreateDto> DecomposeDicomToImagen(Dictionary<DicomTag, object> metadata);



    }
}
