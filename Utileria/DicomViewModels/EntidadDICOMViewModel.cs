using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utileria.Listados;

namespace Utileria.DicomViewModels
{
    public class EntidadDICOMViewModel
    {
        public string Host { get; set; } // Ip
        public int Port { get; set; } // Puerto
        public string CallingAe { get; set; } // AETitle Client
        public TipoEntidad Tipo { get; set; }
        public enum TipoEntidad
        {
            Server,
            Cliente
        }
    }
}
