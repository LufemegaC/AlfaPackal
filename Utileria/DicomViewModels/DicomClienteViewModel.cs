using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utileria.DicomViewModels
{
    public class DicomClienteViewModel : EntidadDICOMViewModel
    {
        public string CalledAe { get; set; } // AETitle Server
        public IFormFile DicomFile { get; set; } // Para la carga de archivos
    }
}
