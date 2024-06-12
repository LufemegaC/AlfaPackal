using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;

namespace Api_PACsServer.Services.IService
{
    public interface IGeneralAPIService
    {
        Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues mainEntitiesValues);

        // Listado principal
        Task<List<EstudioConPacienteDto>> ObtenerEstudiosConPaciente();

        // Listado principal
        Task<List<StudyListVM>> GetStudyList(int institutionId);

        //Adaptacion de consultas para atender C-FIND
        /// <summary>
        /// Searches in a DICOM store for patient information. Returns a representative DICOM file per found patient
        /// </summary>
        Task<List<string>> FindPatientFiles(string patientName, string patientId);

        /// <summary>
        /// Searches in a DICOM store for study information. Returns a representative DICOM file per found study
        /// </summary>
        Task<List<string>> FindStudyFiles(string PatientName, string PatientId, string AccessionNbr, string StudyUID);

        /// <summary>
        /// Searches in a DICOM store for series information. Returns a representative DICOM file per found serie
        /// </summary>
        Task<List<string>> FindSeriesFiles(string PatientName, string PatientId, string AccessionNbr, string StudyUID, string SeriesUID, string Modality);

        /// <summary>
        /// Searches in a DICOM store for all files matching the given UIDs
        /// </summary>
        Task<List<string>> FindFilesByUID(string PatientId, string StudyUID, string SeriesUID);

        
    }
}
