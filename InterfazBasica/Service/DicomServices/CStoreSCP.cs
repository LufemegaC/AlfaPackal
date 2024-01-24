using AutoMapper;
using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service;
using InterfazBasica.Service.IService;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utileria;
using static Utileria.Listados;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        private IEstudioService _estudioService;
        private readonly IMapper _mapper;

        private DicomFile dicomFile;

        private static readonly DicomTransferSyntax[] _acceptedTransferSyntaxes = new DicomTransferSyntax[]
            {
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
            };

        private static readonly DicomTransferSyntax[] _acceptedImageTransferSyntaxes = new DicomTransferSyntax[]
        {
               // Lossless
               DicomTransferSyntax.JPEGLSLossless,
               DicomTransferSyntax.JPEG2000Lossless,
               DicomTransferSyntax.JPEGProcess14SV1,
               DicomTransferSyntax.JPEGProcess14,
               DicomTransferSyntax.RLELossless,
               // Lossy
               DicomTransferSyntax.JPEGLSNearLossless,
               DicomTransferSyntax.JPEG2000Lossy,
               DicomTransferSyntax.JPEGProcess1,
               DicomTransferSyntax.JPEGProcess2_4,
               // Uncompressed
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
        };


        public CStoreSCP(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies, IEstudioService estudioService, IMapper mapper)
            : base(stream, fallbackEncoding, log, dependencies)
        {
            _estudioService = estudioService;
            _mapper = mapper;
        }

                    
        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            if (association.CalledAE != "STORESCP")
            {
                return SendAssociationRejectAsync(
                    DicomRejectResult.Permanent,
                    DicomRejectSource.ServiceUser,
                    DicomRejectReason.CalledAENotRecognized);
            }

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                {
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
            }

            return SendAssociationAcceptAsync(association);
        }


        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            return SendAssociationReleaseResponseAsync();
        }


        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            /* nothing to do here */
        }


        public void OnConnectionClosed(Exception exception)
        {
            /* nothing to do here */
        }

        /*
         * [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearVilla(VillaCreateDto modelo)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.Crear<APIResponse>(modelo);
                if(response != null && response.IsExitoso)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(modelo);
        }
         * */

        public async Task<DicomCStoreResponse> OnCStoreRequestAsync(DicomCStoreRequest request)
        {
            // Extraer UID de estudio e instancia
            try
            {
                var studyUid = request.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID).Trim();
                var instUid = request.SOPInstanceUID.UID;
                var seriesUid = request.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID).Trim();

                EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(request.Dataset);
                var response = await _estudioService.Crear<APIResponse>(estudioDto);
                if (response != null && response.IsExitoso)
                { 
                    // Insetar Series
                }
                var path = Path.Combine(Path.GetFullPath(DS.RutaAlmacen), studyUid, seriesUid);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, instUid + ".dcm");

                await request.File.SaveAsync(path);

                return new DicomCStoreResponse(request, DicomStatus.Success);
            }
            catch (Exception ex)
            {
                return new DicomCStoreResponse(request, DicomStatus.Warning);
            }
            


            //// Mapear y guardar la información del estudio
            //EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(request.Dataset);
            //response = await _estudioService.Crear<APIResponse>(estudioDto);
            //if (response != null && response.IsExitoso)
            //{
            //    var idEstudioPacs = 
            //    // Mapear y guardar la información de la serie
            //    SerieCreateDto serieDto = _mapper.Map<SerieCreateDto>(request.Dataset);
            //    serieDto.PACS_EstudioID = _estudioService.Obtener(studyUid); // Suponiendo que tienes una forma de obtener el ID del estudio
            //    await GuardarSerie(serieDto);

            //}

            //// Mapear y guardar la información de la imagen
            //ImagenCreateDto imagenDto = _mapper.Map<ImagenCreateDto>(request.Dataset);
            //imagenDto.PACS_SerieID = ObtenerSerieIDDesdeUID(seriesUid); // Suponiendo que tienes una forma de obtener el ID de la serie
            //await GuardarImagen(imagenDto, path); // Guarda la imagen y su ubicación

            // Guardar el archivo físico de la imagen
            
        }


        public Task OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // let library handle logging and error response
            return Task.CompletedTask;
        }


        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }


    }
}
