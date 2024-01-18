using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Azure;
using static Utileria.DicomUtilities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerieController : ControllerBase
    {
        private readonly ILogger<SerieController> _logger;
        private readonly ISerieRepositorio _serieRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public SerieController(ILogger<SerieController> logger, ISerieRepositorio serieRepo, IMapper mapper)
        {
            _logger = logger;
            _serieRepo = serieRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        // GET: api/Serie
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetSeries()
        {
            try
            {
                var listaSeries = await _serieRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<SerieDto>>(listaSeries);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // GET: api/Serie/5
        [HttpGet("{id:int}", Name = "GetSerieById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var serie = await _serieRepo.ObtenerPorID(v => v.PACS_SerieID == id);
                if (serie == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<SerieDto>(serie);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // GET: api/Serie/5
        [HttpGet("{instanceUID}", Name = "GetSerieByIstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieByIstanceUID(string instanceUID)
        {
            try
            {
                if (!IsValidDicomUid(instanceUID))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var serie = await _serieRepo.GetSerieByInstanceUID(instanceUID);
                if (serie == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<SerieDto>(serie);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        // POST: api/Serie
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearSerie([FromBody] SerieCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid || createDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!IsValidDicomUid(createDto.SeriesInstanceUID))
                {
                    return BadRequest("El formato de la instancia UID no es valido.");
                }
                // Validación específica para StudyInstanceUID
                if (await _serieRepo.ExisteSeriesInstanceUID(createDto.SeriesInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo StudyInstanceUID.");
                }
                var serie = _mapper.Map<Serie>(createDto);
                await _serieRepo.Crear(serie);
                _response.Resultado = serie;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetSerie", new { id = serie.PACS_SerieID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // 18/01/24.-Luis Felipe MG: Codigo comentado en esta version demo
        // PUT: api/Serie/5
        //[HttpPut("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateSerie(int id, [FromBody] SerieUpdateDto updateDto)
        //{
        //    try
        //    {
        //        if (updateDto == null || id != updateDto.PACS_SerieID)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }

        //        var serieExistente = await _serieRepo.ObtenerPorID(x => x.PACS_SerieID == id);
        //        if (serieExistente == null)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }

        //        _mapper.Map(updateDto, serieExistente);
        //        await _serieRepo.Actualizar(serieExistente);
        //        _response.StatusCode = HttpStatusCode.NoContent;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return BadRequest(_response);
        //}

        // DELETE: api/Serie/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var serie = await _serieRepo.ObtenerPorID(v => v.PACS_SerieID == id);
                if (serie == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _serieRepo.Remover(serie);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        // PATCH: api/Serie/5
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialSerie(int id, JsonPatchDocument<SerieUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id <= 0)
                {
                    return BadRequest();
                }

                var serieExistente = await _serieRepo.ObtenerPorID(x => x.PACS_SerieID == id, tracked: false);
                if (serieExistente == null)
                {
                    return NotFound("Serie no encontrada.");
                }

                SerieUpdateDto serieDto = _mapper.Map<SerieUpdateDto>(serieExistente);
                patchDto.ApplyTo(serieDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(serieDto, serieExistente);
                await _serieRepo.Actualizar(serieExistente);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}
