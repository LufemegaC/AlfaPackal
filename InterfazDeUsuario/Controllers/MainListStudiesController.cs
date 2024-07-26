using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.Identity;
using InterfazDeUsuario.Models.ViewModels;
using InterfazDeUsuario.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Utileria;

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
            var studiesMainList = new List<StudyListVM>();
            ListadoPrincipalVM listadoPrincipalVM = new ListadoPrincipalVM();
            // Validacion de pagina uno
            if (pageNumber < 1) pageNumber = 1;
            var response = await _dataService.GetMainListPaginado<APIResponse>(tokenV, InstitutionId, pageNumber, 10);
            if (response != null && response.IsExitoso)
            {
                studiesMainList = JsonConvert.DeserializeObject<List<StudyListVM>>(Convert.ToString(response.Resultado));
                listadoPrincipalVM = new ListadoPrincipalVM()
                {
                    StudyList = studiesMainList,
                    PageNumber = pageNumber,
                    TotalPaginas = JsonConvert.DeserializeObject<int>(Convert.ToString(response.TotalPaginas))
                };
                if (pageNumber > 1) listadoPrincipalVM.Previo = "";
                if (listadoPrincipalVM.TotalPaginas <= pageNumber) listadoPrincipalVM.Siguiente = "disabled";
            }
            return View(listadoPrincipalVM);

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
                    _token = HttpContext.Session.GetString(DS.SessionToken);
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
                    _institution = HttpContext.Session.GetInt32(DS.Institution) ?? 0;
                }
                return _institution;
            }
        }

    }
}
