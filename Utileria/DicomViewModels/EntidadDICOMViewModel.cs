using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utileria.DicomValues;

namespace Utileria.DicomViewModels
{
    public class EntidadDICOMViewModel
    {
        public string Host { get; set; } // Ip
        public int Port { get; set; } // Puerto
        public string? Aet { get; set; } // Aetitle
        public TipoEntidad Tipo { get; set; }
        public enum TipoEntidad
        {
            Server,
            Cliente
        }
    }
}
