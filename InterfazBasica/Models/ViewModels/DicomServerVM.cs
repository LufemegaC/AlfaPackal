using InterfazBasica_DCStore.Models.Pacs;
using Utileria.DicomViewModels;
using static Utileria.DicomValues;

namespace InterfazBasica_DCStore.Models.ViewModels
{
    public class DicomServerVM : EntidadDICOMViewModel
    {
        public DicomServerVM() {
            Tipo = TipoEntidad.Server;
        }
        public DicomServerDto DicomServerDto { get; set; }
        public bool IsListening { get; set; }
        public string Estatus { get; set; }
    }
}
