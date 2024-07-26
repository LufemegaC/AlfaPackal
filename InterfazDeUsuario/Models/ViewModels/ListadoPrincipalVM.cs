namespace InterfazDeUsuario.Models.ViewModels
{
    public class ListadoPrincipalVM
    {
        public int PageNumber { get; set; }
        public int TotalPaginas { get; set; }
        public string Previo { get; set; } = "disabled";
        public string Siguiente { get; set; } = "";

        public IEnumerable<StudyListVM> StudyList { get; set; }
    }
}
