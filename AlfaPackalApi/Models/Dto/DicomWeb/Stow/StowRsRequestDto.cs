using FellowOakDicom;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.DicomWeb.Stow
{
    public class StowRsRequestDto
    {

        public List<DicomFilePackage> DicomFilesPackage { get; set; }

        public string TransactionUID { get; set; }
    }
}
