using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.Dtos.PacsDto;
using InterfazDeUsuario.Services.IServices;
using InterfazDeUsuario.Utility;
using InterfazDeUsuario.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterfazDeUsuario.Controllers
{
    public class MainListStudiesController : Controller
    {
        private readonly IDataService _dataService;
        private string _token;
        private int _institution;

        public MainListStudiesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> IndexMainList(int pageNumber = 1)
        {
            //ListadoPrincipalVM mainList = new();
            string tokenV = Token;
            int institution = InstitutionId;
            var studyListVM = new StudiesMainListVM();
            // Validacion de pagina uno
            if (pageNumber < 1) pageNumber = 1;
            var response = await _dataService.GetMainListPaginado<APIResponse>(tokenV, institution, pageNumber, 10);
            if (response != null && response.IsSuccessful)
            {
                // Deserializar la lista de estudios desde la respuesta
                var studiesMainList = JsonConvert.DeserializeObject<IEnumerable<StudyDto>>(Convert.ToString(response.ResponseData));
                // Crear el ViewModel con los datos necesarios para la vista
                studyListVM.StudyList = studiesMainList;
                studyListVM.PageNumber = pageNumber;
                studyListVM.TotalPages = JsonConvert.DeserializeObject<int>(Convert.ToString(response.TotalPages));
                studyListVM.Prev = pageNumber > 1 ? "" : "disabled";
                studyListVM.Next = studyListVM.TotalPages > pageNumber ? "" : "disabled";
            }
            return View(studyListVM);

        }

        [HttpPost]
        public IActionResult GetStudy([FromBody] dynamic data)
        {
            try
            {
                string studyUID = data.id;
                // Lógica para manejar la ejecución del método
                // Puedes utilizar los datos recibidos (en este caso, el ID) según sea necesario

                // Ejemplo: Llamar a un servicio para procesar el ID
                //_wadoUriService.AlgunMetodo(id);
                TempData["studyUID"] = studyUID;
                return RedirectToAction("MainVisualizer", "DicomVisualizer");
                //return Json(new { success = true, message = "Método ejecutado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token))
                {
                    _token = HttpContext.Session.GetString(LocalUtility.SessionToken);
                }
                return _token;
            }
        }

        public int InstitutionId
        {
            get
            {
                if (_institution == 0 )
                {
                    _institution = HttpContext.Session.GetInt32(LocalUtility.SessionInstitution) ?? 0;
                }
                return _institution;
            }
        }

    }
}
