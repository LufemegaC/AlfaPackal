using Utileria.DicomViewModels;
using static Utileria.DicomValues;

namespace InterfazBasica_DCStore.Models.ViewModels
{
    public class DicomServerVM : EntidadDICOMViewModel
    {
        public DicomServerVM() {
            Tipo = TipoEntidad.Server;
        }

        public bool IsListening { get; set; }

        public string Estatus { get; set; }
    }
}
