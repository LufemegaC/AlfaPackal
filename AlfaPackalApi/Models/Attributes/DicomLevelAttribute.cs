using FellowOakDicom.Network;

namespace Api_PACsServer.Models.Attributes
{
    public class DicomLevelAttribute : Attribute
    {
        public DicomQueryRetrieveLevel Level { get; } // Aquí se guarda la categoría (nivel DICOM).

        public DicomLevelAttribute(DicomQueryRetrieveLevel level)
        {
            Level = level; // Al crear la etiqueta, se escribe la categoría.
        }
    }
}
