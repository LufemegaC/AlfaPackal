using AutoMapper;
using FellowOakDicom;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace InterfazBasica_DCStore.Controllers
{
    public class DemoController : Controller
    {
        private IEstudioService _estudioService;
        private readonly IMapper _mapper;

        public DemoController(IEstudioService estudioService, IMapper mapper)
        {
            _estudioService = estudioService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexEstudio()
        {
            //List <EstudioDto> estudioList = new();
            //var response = await _estudioService.GetAll<APIResponse>();
            //if (response != null && response.IsExitoso)
            //{
            //    estudioList = JsonConvert.DeserializeObject<List<EstudioDto>>(Convert.ToString(response.Resultado));
            //}
            return View();
        }

        //Get
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudio(EstudioCreateDto modelo)
        {
           // EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(request.Dataset);
            //if (ModelState.IsValid)
            //{
            //    var response = await _estudioService.Create<APIResponse>(modelo);
            //    if (response != null && response.IsExitoso)
            //    {
            //        return RedirectToAction(nameof(IndexEstudio));
            //    }
            //}
            return View();
        }
    }
}
