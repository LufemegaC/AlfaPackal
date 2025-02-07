using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Studies
{
    public class StudyModalityCreateDto
    {
        [Required]
        public string StudyInstanceUID { get; set; }

        [Required]
        public string Modality { get; set; }
    }
}
