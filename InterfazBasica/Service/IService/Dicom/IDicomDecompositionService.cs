using FellowOakDicom;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using Newtonsoft.Json.Linq;

namespace InterfazBasica_DCStore.Service.IService.Dicom
{
    public interface IDicomDecompositionService
    {
        ///// <summary>
        ///// Extracts metadata from a DICOM dataset and returns it as a dictionary.
        ///// </summary>
        ///// <param name="dicomDataset">The DICOM dataset from which metadata is to be extracted.</param>
        ///// <returns>A dictionary containing the extracted metadata with DICOM tags as keys and corresponding values.</returns>
        //Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset);

        ///// <summary>
        ///// Converts a dictionary of DICOM metadata into a DTO containing the main entities (Study, Series, and Instance) for creation.
        ///// </summary>
        ///// <param name="metadata">The dictionary of DICOM metadata.</param>
        ///// <returns> contains the DTO for creating the main entities.</returns>
        //MetadataDto DicomDictionaryToCreateEntities(Dictionary<DicomTag, object> metadata);

        /// <summary>
        /// Converts a DICOM dataset to its JSON representation.
        /// </summary>
        /// <param name="dicomDataset">The DICOM dataset.</param>
        /// <returns>A JSON string representation of the dataset.</returns>
        string ConvertDatasetToJson(DicomDataset dicomDataset);

        ///// <summary>
        ///// Calculates the file size of a DICOM file in megabytes (MB).
        ///// </summary>
        ///// <param name="dicomFile">The DICOM file from which to calculate the size.</param>
        ///// <returns>The size of the DICOM file in megabytes (MB).</returns>
        //decimal GetFileSizeInMB(DicomFile dicomFile);
    }
}
