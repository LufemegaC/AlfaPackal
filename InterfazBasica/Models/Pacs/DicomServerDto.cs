using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InterfazBasica_DCStore.Models.Pacs
{
    public class DicomServerDto
    {
        public string IP { get; set; }
        public string AETitle { get; set; }
        public int PuertoRedLocal { get; set; }
        public string Description { get; set; }
        public int InstitutionId { get; set; }
        //Union con institution
    }
}
