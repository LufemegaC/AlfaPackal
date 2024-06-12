using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.ViewModels;
using InterfazDeUsuario.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utileria;

namespace InterfazDeUsuario.Controllers
{
    public class MainListStudiesController : Controller
    {
        private readonly IDataService _dataService;
        private string _token;

        public MainListStudiesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> IndexMainList()
        {
            //ListadoPrincipalVM mainList = new();
            string tokenV = "";
            var studiesMainList = new List<StudyListVM>();
            var response = await _dataService.GetMainList<APIResponse>(tokenV);
            if (response != null && response.IsExitoso)
            {
                studiesMainList = JsonConvert.DeserializeObject<List<StudyListVM>>(Convert.ToString(response.Resultado));
            }
            return View(studiesMainList);
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
    }
}
