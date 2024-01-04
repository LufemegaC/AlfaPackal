using Utileria.DicomViewModels;
using static Utileria.Listados;

namespace InterfazBasica_DCStore.Models.ViewModels
{
    public class DicomServerVM : EntidadDICOMViewModel
    {
        public DicomServerVM() {
            Tipo = TipoEntidad.Server;
        }
    }
}
