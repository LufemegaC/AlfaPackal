using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.DicomWeb
{
    public class StowRsRequestDto
    {

        /// <summary>
        /// The DICOM file uploaded.
        /// </summary>
        [Required]
        public IFormFile DicomFile { get; set; }

        /// <summary>
        /// The metadata associated with the DICOM file.
        /// </summary>
        public MetadataDto Metadata { get; set; }

    }
}
