using Utileria.DicomViewModels;
using static Utileria.DicomValues;

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
