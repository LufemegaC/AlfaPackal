using Utileria.DicomViewModels;
using static Utileria.Listados;

namespace WorkStation.Models.ViewModels
{
    public class DicomClienteVM : DicomClienteViewModel
    {
        public DicomClienteVM()
        {
            Tipo = TipoEntidad.Cliente;
        }
        
    }
}
