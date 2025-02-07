using FellowOakDicom;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.DicomWeb.Stow
{
    public class DicomFilePackage
    {
        /// <summary>
        /// The DICOM file uploaded.
        /// </summary>
        [Required]
        public DicomFile DicomFile { get; set; }

        /// <summary>
        /// The metadata associated with the DICOM file.
        /// </summary>
        public MetadataDto Metadata { get; set; }

    }
}
