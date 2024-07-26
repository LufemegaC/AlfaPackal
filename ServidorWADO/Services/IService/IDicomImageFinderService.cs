namespace ServidorWADO.Services.IService
{
    /// <summary>
    /// represents a service used to retrieve images by instance UID
    /// </summary>
    public interface IDicomImageFinderService
    {
        /// <summary>
        /// Returns the image path of the dicom file with instance UID = instanceUid
        /// </summary>
        /// <param name="instanceUid">instance uid of the image to find</param>
        /// <returns>the image path if found, else null</returns>
        string GetImageByInstanceUid(string instanceUid);
        /// <summary>
        /// Returns the image path of the dicom file with instance UID = instanceUid
        /// </summary>
        /// <param name="studyUid">instance study uid of the image to find</param>
        /// <param name="SerieUid">instance serie uid of the image to find</param>
        /// <param name="instanceUid">instance uid of the image to find</param>
        /// <returns>the image path if found, else null</returns>
        string GetImageByInstanceUid(string studyUid, string SerieUid, string instanceUid);
    }
}
