using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Models.ViewModels;
using InterfazBasica_DCStore.Service;
using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utileria;

namespace InterfazBasica_DCStore.Controllers
{
    public class MainListController : Controller
    {
        private readonly IGeneralAPIServices _generalAPIService;
        private string _token;
        
        public MainListController(IGeneralAPIServices generalAPIService)
        {
            _generalAPIService = generalAPIService;
        }

        public async Task<IActionResult> IndexMainList()
        {
            ListadoPrincipalVM mainList = new();
            var response = await _generalAPIService.GetMainList<APIResponse>(Token);
            if (response != null && response.IsExitoso)
            {
                mainList.MainListStudies = JsonConvert.DeserializeObject<List<EstudioConPacienteDto>>(Convert.ToString(response.Resultado));
            }
            return View(mainList);
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
