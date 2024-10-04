using InterfazDeUsuario.Services.IServices;
using InterfazDeUsuario.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InterfazDeUsuario.Controllers
{
    public class DicomVisualizerController : Controller
    {
        private readonly IWadoUriService _wadoUriService;

        private string _token;

        public DicomVisualizerController(IWadoUriService wadoUriService)
        {
            _wadoUriService = wadoUriService;
        }


        public IActionResult MainVisualizer(string studyUid)
        {
            string studyUID = (string)TempData["studyUID"];
            _wadoUriService.GetInstancesByStudyUIDAsync(Token, "wado", studyUID);
            return View();
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
    }
}
