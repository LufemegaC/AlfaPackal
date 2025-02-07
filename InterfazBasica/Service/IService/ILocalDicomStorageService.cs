using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models.Dicom.Web;

namespace InterfazBasica_DCStore.Service.IService
{
    /// <summary>
    /// Interface for managing the local storage of DICOM files.
    /// </summary>
    public interface ILocalDicomStorageService
    {
        /// <summary>
        /// Stores the DICOM file locally, applying the specified convention or location (e.g., folder, database, etc.).
        /// </summary>
        /// <param name="dicomFile">The DICOM instance to store.</param>
        /// <returns>A task that represents the completion of the storage operation, returning the status of the operation.</returns>
        Task<DicomStatus> StoreDicomFileAsync(DicomFile dicomFile, string rootPath);

    }

}
