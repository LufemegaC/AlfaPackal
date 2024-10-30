using InterfazDeUsuario.Services.IDicomWeb;
using InterfazDeUsuario.Utilities;
using InterfazDeUsuario.Utility;
using InterfazDeUsuario.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace InterfazDeUsuario.Controllers
{
    public class MainListStudiesController : Controller
    {
        private readonly IDicomWebService _dicomWebService;
        private string _token;
        private int _institution;

        public MainListStudiesController(IDicomWebService dicomWebService)
        {
            _dicomWebService = dicomWebService;
            //_OHIFService = OHIFService;
        }
        // API SERVICEE
        public async Task<IActionResult> IndexMainList(int pageNumber = 1)
        {
            //ListadoPrincipalVM mainList = new();
            string tokenV = Token;
            //int institution = InstitutionId;
            var studyListVM = new StudiesMainListVM();
            // Validacion de pagina uno
            if (pageNumber < 1) pageNumber = 1;
            var response = await _dicomWebService.GetMainListPaginado(tokenV, 25, pageNumber, 10);
            if (response != null)
            {
                // Deserializar la lista de estudios desde la respuesta
                var studiesMainList = DicomWebHelper.ConvertQidoJsonToDtos(response);
                // Crear el ViewModel con los datos necesarios para la vista
                studyListVM.StudyList = studiesMainList;
                studyListVM.PageNumber = pageNumber;
                //studyListVM.TotalPages = JsonConvert.DeserializeObject<int>(Convert.ToString(response.TotalPages));
                studyListVM.Prev = pageNumber > 1 ? "" : "disabled";
                //studyListVM.Next = studyListVM.TotalPages > pageNumber ? "" : "disabled";
            }
            return View(studyListVM);

        }

        // OHIF
        //public async Task<IActionResult> SendStudyInfoToOHIF(string studyInstanceUID)
        //{
        //    string tokenV = Token;
        //    var response = await _dataService.GetInfoStudy<APIResponse>(tokenV, studyInstanceUID);
        //    if (response != null && response.IsSuccessful)
        //    {
        //        // Serialize the ResponseData back to a JSON string
        //        var jsonString = JsonConvert.SerializeObject(response.ResponseData);

        //        // Configure serializer settings to handle camelCase property names
        //        var settings = new JsonSerializerSettings
        //        {
        //            ContractResolver = new DefaultContractResolver
        //            {
        //                NamingStrategy = new CamelCaseNamingStrategy()
        //            }
        //        };

        //        // Deserialize the JSON string into the OHIFStudy model
        //        var studyInfo = JsonConvert.DeserializeObject<OHIFStudy>(jsonString, settings);

        //        return View(studyInfo);  // Pass the model to the view
        //    }
        //    return NotFound();
        //}

        ////* POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendStudyInfoToOHIF(OHIFStudy studyInfo)
        //{
        //    // Envía la información en formato JSON al visor OHIF
        //    await _OHIFService.SendStudyDataAsync(studyInfo);
        //    return View();
        //}

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
