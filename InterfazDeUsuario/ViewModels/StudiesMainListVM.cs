using InterfazDeUsuario.Models.Dtos.PacsDto;

namespace InterfazDeUsuario.ViewModels
{
    public class StudiesMainListVM
    {
        public IEnumerable<StudyDto> StudyList { get; set; } // Listado de estudios
        public int PageNumber { get; set; } // Página actual
        public int TotalPages { get; set; } // Total de páginas
        public string Prev { get; set; } // Control de navegación (deshabilitado si no hay página previa)
        public string Next { get; set; } // Control de navegación (deshabilitado si no hay más páginas)


    }
}
