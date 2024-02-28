using AutoMapper;
using FellowOakDicom;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace InterfazBasica_DCStore.Service
{
    public class DicomOrchestratorService
    {
        private IEstudioService _estudioService;
        private readonly IMapper _mapper;

        public DicomOrchestratorService(IEstudioService estudioService, IMapper mapper)
        {
            _estudioService = estudioService;
            _mapper = mapper;
        }

        public void DicomInsertEventHandler(DicomFile dicomFile)
        {
            // Acceder a los metadatos del DicomFile
            string studyInstanceUid = dicomFile.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);
            string seriesInstanceUid = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
            string sopInstanceUid = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SOPInstanceUID);

        }

        //Get
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CrearEstudio(EstudioCreateDto modelo)
        //{
        //    EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(request.Dataset);
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _estudioService.Crear<APIResponse>(modelo);
        //        if (response != null && response.IsExitoso)
        //        {
        //            return RedirectToAction(nameof(IndexEstudio));
        //        }
        //    }
        //    return View(modelo);
        //}

    }
}
