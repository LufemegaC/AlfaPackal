namespace WorkStation.Models.ViewModels
{
    public class ModalityVM
    {
        public DicomClientDto DicomClient { get; set; }
        public IFormFile? DicomFile { get; set; }
    }
}
