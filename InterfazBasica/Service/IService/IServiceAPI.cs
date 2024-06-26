﻿using FellowOakDicom.Network;
using FellowOakDicom;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IServiceAPI
    {
        // ** Metodos de Registro
        // Método para el registro de pacientes
        Task<APIResponse> RegistrarPaciente(PacienteCreateDto modelo);

        // Método para el registro de estudios
        Task<APIResponse> RegistrarEstudio(EstudioCreateDto modelo);

        // Método para el registro de series
        Task<APIResponse> RegistrarSerie(SerieCreateDto modelo);

        // Método para el registro de imágenes
        Task<APIResponse> RegistrarImagen(ImagenCreateDto modelo);

        // ** Metodos de validacion en BDs
        // Metodos Get by ID/UID
        Task<int?> GetPACSIDPatientByName(string name);
        // Get patiente by ID generated by DICOM Server
        Task<int?> GetPACSIDPatientByGeneratedID(string generatedID);
        // Validacion por Study UID
        Task<int?> GetPACSIDStudyByInstanceUID(string studyInstanceUID);
        // Validacion por Serie UID
        Task<int?> GetPACSIDSeriesByInstanceUID(string seriesInstanceUID);
        // Validacion por SOP Instance UID unica en el archivo DICOM
        Task<int?> GetPACSIDImageBySOPInstanceUID(string sopInstanceUID);

        // Adaptacion de interfaz de ejemplo IDicomImageFinderService
        //Task<List<String>>

    } 
}





