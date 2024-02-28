using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace InterfazBasica.Models.Pacs
{
    public class EstudioUpdateDto
    {
        // 16/01/2024 Luis Felipe MG.- Comento toda la clase por que el UPDATE
        // no estará presente en esta version

        ////Llave primaria
        //[Key]
        //public int EstudioID { get; set; }
        ////Paciente
        //public int? PatientID { get; set; }
        ////Doctor
        //public int? DoctorID { get; set; }
        ////Lista de trabajo
        //public int? ListaID { get; set; }
        ////Modalidad
        //[Required, MaxLength(2)]
        //public Modalidad Modality { get; set; }
        //// Descripcion
        //[Required]
        //public string DescripcionEstudio { get; set; }
        //[Required]
        //public DateTime StudyDate { get; set; }
        //[Required]
        //public int TiempoEstudio { get; set; }

        //[Required, MaxLength(100)]
        //public string BodyPartExamined { get; set; }
        //[Required, MaxLength(64)]
        //public string StudyInstanceUID { get; set; }
        //[Required, MaxLength(16)]
        //public string AccessionNumber { get; set; }
    }
}
