using AutoMapper;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace InterfazBasica_DCStore.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteService _pacienteService;
        private readonly IMapper _mapper;
        private readonly PacienteCreateDto _pacienteCreateDto;

        public PacienteController(IPacienteService pacienteService, IMapper mapper)
        {
            _pacienteService = pacienteService;
            _mapper = mapper;
            _pacienteCreateDto = new PacienteCreateDto()
            {
                PatientID = "45521654",
                PatientName = "John Titor",
                PatientAge = "45",
                PatientSex = "M",
                PatientWeight = "50",
                PatientBirthDate = DateTime.Parse("1989-05-10T07:38:24.815Z"),
                IssuerOfPatientID = "Hospital General Gandulfo"
            };
        }
        public async Task<IActionResult> IndexPaciente(PacienteCreateDto modelo)
        {
            //List<PacienteDto> pacienteList = new();
            //var response = await _pacienteService.GetAll<APIResponse>();
            //if (response != null && response.IsExitoso)
            //{
            //    pacienteList = JsonConvert.DeserializeObject<List<PacienteDto>>(Convert.ToString(response.Resultado));
            //}
            return View();
        }

        //Get
        public async Task<IActionResult> CrearPaciente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPaciente(PacienteCreateDto modelo)
        {
            //if (ModelState.IsValid)
            //{
            //    var response = await _pacienteService.Create<APIResponse>(_pacienteCreateDto);
            //    if (response != null && response.IsExitoso)
            //    {
            //        return RedirectToAction(nameof(IndexPaciente));
            //    }
            //}
            return View();
        }
    }
}
