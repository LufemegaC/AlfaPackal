using AutoMapper;
using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica_DCStore.Service.DicomServers;
using System.Text;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class QRService : DicomService, IDicomServiceProvider, IDicomCFindProvider, IDicomCEchoProvider, IDicomCMoveProvider, IDicomCGetProvider, IDicomCStoreProvider
    {
        //private  

        // DEPENDENCIAS QUE QUIERO AGREGAR
        //private IEstudioService _estudioService;
        //private readonly IMapper _mapper;

        // Sintaxis de transferencia DICOM soportados por el servidor ( Datos generales )
        private static readonly DicomTransferSyntax[] _acceptedTransferSyntaxes = new DicomTransferSyntax[]
            {
                DicomTransferSyntax.ExplicitVRLittleEndian,
                DicomTransferSyntax.ExplicitVRBigEndian,
                DicomTransferSyntax.ImplicitVRLittleEndian
            };

        //Sintaxis de transferencia DICOM soportados por el servidor ( Datos de imagen )
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

        // Nombre de la entidad de aplicacion que solicita el servicio
        public string CallingAE { get; protected set; }
        // Nombre de la entidad de aplicacion que fue llamada
        public string CalledAE { get; protected set; }

        //**********************//
        //Constructor ORIGINAL
        public QRService(INetworkStream stream, Encoding fallbackEncoding, Microsoft.Extensions.Logging.ILogger log, DicomServiceDependencies dependencies)
                : base(stream, fallbackEncoding, log, dependencies)
        {
            /* initialization per association can be done here */

        }

        ////Constructor MODIFICADO con inyeccion de dependencias, deja de funcionar el servidor
        //public QRService(INetworkStream stream, Encoding fallbackEncoding, Microsoft.Extensions.Logging.ILogger log, DicomServiceDependencies dependencies, IEstudioService estudioService, IMapper mapper)
        //        : base(stream, fallbackEncoding, log, dependencies)
        //{
        //    _estudioService = estudioService;
        //    _mapper = mapper;
        //}
        //**********************//


        // Maneja las solicitudes de verificación DICOM C-ECHO
        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            Logger.LogInformation($"Received verification request from AE {CallingAE} with IP: {Association.RemoteHost}");
            //Logger.Info();
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }

        public Task<DicomCEchoResponse> OnCEchoRequestAzureAsync(DicomCEchoRequest request)
        {

            Logger.LogInformation($"Received verification request from AE {CallingAE} with IP: {Association.RemoteHost}");
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }

        // Limpia los recursos cuando la conexión se cierra
        public void OnConnectionClosed(Exception exception)
        {
            Clean();
        }

        //Evento de aborot del servidor por solicitud
        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            //log the abort reason
            Logger.LogError($"Received abort from {source}, reason is {reason}");
        }

        // Limpia los recursos cuando se recibe una solicitud de liberación de asociación
        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            Clean();
            return SendAssociationReleaseResponseAsync();
        }
        /*
		Tipo: DICOM C-FIND
		Recibe: DicomCFindRequest
		Responde: DicomCFindResponse
		Descripcion:se usa para responder a una solicitud asociacion 
		Necesita 'DestinationAE','destinationPort' y 'destinationIP'
		*/

        // Maneja las situaciones cuando se recibe una solicitud de asociación DICOM
        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            //Respaldo de informacion
            CallingAE = association.CallingAE;
            CalledAE = association.CalledAE;

            // Imprime un mensaje de log con la información de la solicitud de asociación
            Logger.LogInformation($"Received association request from AE: {CallingAE} with IP: {association.RemoteHost} ");

            // Comprueba si el AE llamado coincide con el AE del servidor
            if (QRServer.AETitle != CalledAE)
            {
                // Imprime un mensaje de error y envía un rechazo de asociación si el AE llamado es desconocido
                Logger.LogError($"Association with {CallingAE} rejected since called aet {CalledAE} is unknown");
                return SendAssociationRejectAsync(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            }

            //Recorre los contextos de presentación en la asociación
            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelMove
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMove)
                {
                    // Si es así, acepta los sintaxis de transferencia
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelGet
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelGet)
                {
                    // Acepta los sintaxis de transferencia de imagen si el contexto de presentación es para obtener información
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    // Acepta los sintaxis de transferencia de imagen si el contexto de presentación es para la categoría de almacenamiento que no es None
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else
                {
                    // Si el contexto de presentación no es soportado, registra una advertencia y establece el resultado a Rechazar
                    Logger.LogWarning($"Requested abstract syntax {pc.AbstractSyntax} from {CallingAE} not supported");
                    //Logger.Warn($"Requested abstract syntax {pc.AbstractSyntax} from {CallingAE} not supported");
                    pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);
                }
            }
            // Imprime un mensaje de log que la solicitud de asociación ha sido aceptada

            Logger.LogInformation($"Accepted association request from {CallingAE}");
            // Envía una respuesta de aceptación de asociación
            return SendAssociationAcceptAsync(association);
        }
        /*
		Tipo: DICOM C-FIND
		Recibe: DicomCFindRequest
		Responde: DicomCFindResponse
		Descripcion:se usa para responder a una solicitud C-FIND ( Busqueda )
		Necesita 'DestinationAE','destinationPort' y 'destinationIP'
		*/
        // Metodo de solicitud de informacion por parte del SCU
        public async IAsyncEnumerable<DicomCFindResponse> OnCFindRequestAsync(DicomCFindRequest request)
        {
            //Nivel de consulta
            var queryLevel = request.Level;

            // Crear una lista para almacenar las rutas de los archivos que coincidan con los criterios de búsqueda
            var matchingFiles = new List<string>();
            // Crea una instancia del servicio de búsqueda de imágenes DICOM
            IDicomImageFinderService finderService = QRServer.CreateFinderService;

            // a QR SCP has to define in a DICOM Conformance Statement for which dicom tags it can query
            // depending on the level of the query. Below there are only very few parameters evaluated.

            // Dependiendo del nivel de consulta, se evalúan diferentes parámetros
            switch (queryLevel)
            {
                case DicomQueryRetrieveLevel.Patient:
                    {
                        // Recupera el nombre y la identificación del paciente de la solicitud
                        var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
                        var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
                        // Busca archivos que coincidan con el nombre y la identificación del paciente
                        matchingFiles = finderService.FindPatientFiles(patname, patid);
                    }
                    break;

                case DicomQueryRetrieveLevel.Study:
                    {
                        // Recupera los parámetros de la solicitud
                        var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
                        var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
                        var accNr = request.Dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty);
                        var studyUID = request.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty);

                        // Busca archivos que coincidan con los parámetros
                        matchingFiles = finderService.FindStudyFiles(patname, patid, accNr, studyUID);
                    }
                    break;

                case DicomQueryRetrieveLevel.Series:
                    {
                        // Recupera los parámetros de la solicitud
                        var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
                        var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
                        var accNr = request.Dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty);
                        var studyUID = request.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty);
                        var seriesUID = request.Dataset.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, string.Empty);
                        var modality = request.Dataset.GetSingleValueOrDefault(DicomTag.Modality, string.Empty);

                        // Busca archivos que coincidan con los parámetros
                        matchingFiles = finderService.FindSeriesFiles(patname, patid, accNr, studyUID, seriesUID, modality);
                    }
                    break;

                case DicomQueryRetrieveLevel.Image:
                    // Para el nivel de consulta de imagen, regresa una respuesta indicando que no puede procesar la solicitud
                    yield return new DicomCFindResponse(request, DicomStatus.QueryRetrieveUnableToProcess);
                    yield break;
            }

            // now read the required dicomtags from the matching files and return as results
            // Para cada archivo que coincida, lee los tags DICOM requeridos y los retorna como resultados
            foreach (var matchingFile in matchingFiles)
            {
                var dicomFile = DicomFile.Open(matchingFile);
                var result = new DicomDataset();
                foreach (var requestedTag in request.Dataset)
                {
                    // most of the requested DICOM tags are stored in the DICOM files and therefore saved into a database.
                    // you can fill the responses by selecting the values from the database.
                    // also be aware that there are some requested DicomTags like "ModalitiesInStudy" or "NumberOfStudyRelatedInstances" 
                    // or "NumberOfPatientRelatedInstances" and so on which have to be calculated and cannot be read from a DICOM file.
                    if (dicomFile.Dataset.Contains(requestedTag.Tag))
                    {
                        // Comprueba si el archivo DICOM contiene el tag solicitado
                        dicomFile.Dataset.CopyTo(result, requestedTag.Tag);
                    }
                    // else if (requestedTag == DicomTag.NumberOfStudyRelatedInstances)
                    // {
                    //    ... somehow calculate how many instances are stored within the study
                    //    result.Add(DicomTag.NumberOfStudyRelatedInstances, number);
                    // } ....
                    else
                    {
                        // Si no existe el tag, añade el tag solicitado al conjunto de resultados
                        result.Add(requestedTag);
                    }
                }
                // Regresa una respuesta con estado Pendiente y el conjunto de resultados
                yield return new DicomCFindResponse(request, DicomStatus.Pending) { Dataset = result };
            }
            // Regresa una respuesta con estado Éxito al final
            yield return new DicomCFindResponse(request, DicomStatus.Success);
        }


        public void Clean()
        {
            // cleanup, like cancel outstanding move- or get-jobs
        }

        /*
		Tipo: DICOM C-MOVE
		Recibe: DicomCMoveRequest
		Responde: DicomCMoveResponse
		Descripcion: Funcion para mover un conjunto de imagenes DICOM a otra entidad dentro de la misma
		red DICOM.
		Necesita 'DestinationAE','destinationPort' y 'destinationIP'
		*/
        public async IAsyncEnumerable<DicomCMoveResponse> OnCMoveRequestAsync(DicomCMoveRequest request)
        {
            // the c-move request contains the DestinationAE. the data of this AE should be configured somewhere.
            // La solicitud c-move contiene el DestinationAE. Los datos de este AE deberían estar configurados en algún lugar.
            if (request.DestinationAE != "STORESCP")
            {
                // Si el DestinationAE no es "STORESCP", se retorna una respuesta con estado de "Destino desconocido" y "Fallo en el procesamiento".
                yield return new DicomCMoveResponse(request, DicomStatus.QueryRetrieveMoveDestinationUnknown);
                yield return new DicomCMoveResponse(request, DicomStatus.ProcessingFailure);
                yield break;
            }

            // this data should come from some data storage!
            // ¡Estos datos deberían provenir de algún almacenamiento de datos!
            var destinationPort = 11112;
            var destinationIP = "localhost";

            // Se crea un servicio de búsqueda de imágenes DICOM.
            IDicomImageFinderService finderService = QRServer.CreateFinderService;
            // Se inicializa una lista vacía de archivos correspondientes.
            IEnumerable<string> matchingFiles = Enumerable.Empty<string>();

            // Dependiendo del nivel de la solicitud, se buscan los archivos correspondientes.
            switch (request.Level)
            {
                case DicomQueryRetrieveLevel.Patient:
                    matchingFiles = finderService.FindFilesByUID(request.Dataset.GetString(DicomTag.PatientID), string.Empty, string.Empty);
                    break;

                case DicomQueryRetrieveLevel.Study:
                    matchingFiles = finderService.FindFilesByUID(string.Empty, request.Dataset.GetString(DicomTag.StudyInstanceUID), string.Empty);
                    break;

                case DicomQueryRetrieveLevel.Series:
                    matchingFiles = finderService.FindFilesByUID(string.Empty, string.Empty, request.Dataset.GetString(DicomTag.SeriesInstanceUID));
                    break;
                // Si el nivel es 'Image', se retorna una respuesta indicando incapacidad para realizar suboperaciones.
                case DicomQueryRetrieveLevel.Image:
                    yield return new DicomCMoveResponse(request, DicomStatus.QueryRetrieveUnableToPerformSuboperations);
                    yield break;
            }
            // Se crea un cliente DICOM para el IP y puerto de destino, sin uso de TLS, y se negocian operaciones asíncronas.
            var client = DicomClientFactory.Create(destinationIP, destinationPort, false, QRServer.AETitle, request.DestinationAE);
            client.NegotiateAsyncOps();

            // Se cuentan los archivos correspondientes y se inicializan contadores para el almacenamiento realizado y fallido.
            int storeTotal = matchingFiles.Count();
            int storeDone = 0; // this variable stores the number of instances that have already been sent
            int storeFailure = 0; // this variable stores the number of faulues returned in a OnResponseReceived

            // Se itera sobre los archivos correspondientes y se envían solicitudes de almacenamiento para cada uno.
            foreach (string file in matchingFiles)
            {
                var storeRequest = new DicomCStoreRequest(file);
                // !!! there is a Bug in fo-dicom 3.0.2 that the OnResponseReceived handlers are invoked not until the DicomClient has already
                //     sent all the instances. So the counters are not increased image by image sent but only once in a bulk after all storage
                //     has been finished. This bug will be fixed hopefully soon.
                // !!! Hay un error en fo-dicom 3.0.2 en el que los manejadores de OnResponseReceived no se invocan hasta que el DicomClient ya
                //     ha enviado todas las instancias. Por lo tanto, los contadores no se incrementan imagen por imagen enviada, sino solo una vez en bloque después de que se haya terminado todo el almacenamiento.
                //     Esperamos que este error se corrija pronto.

                // En este punto, se controla la respuesta a cada solicitud de almacenamiento. Si el estado de la respuesta es 'éxito', se incrementa el contador de almacenamientos realizados. Si no, se incrementa el contador de fallos.
                storeRequest.OnResponseReceived += (req, resp) =>
                {
                    if (resp.Status == DicomStatus.Success)
                    {
                        Logger.LogInformation("Storage of image successfull");
                        storeDone++;
                    }
                    else
                    {
                        Logger.LogError("Storage of image failed");
                        storeFailure++;
                    }
                };
                client.AddRequestAsync(storeRequest).Wait();
            }
            // Se inicia la tarea de envío de las solicitudes de almacenamiento.
            var sendTask = client.SendAsync();
            // Mientras se esté realizando el envío, se informa al QR SCU cada 2 segundos sobre el estado
            while (!sendTask.IsCompleted)
            {
                // while the send-task is runnin we inform the QR SCU every 2 seconds about the status and how many instances are remaining to send. 
                yield return new DicomCMoveResponse(request, DicomStatus.Pending) { Remaining = storeTotal - storeDone - storeFailure, Completed = storeDone };
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            // Se registra que la tarea ha finalizado.
            Logger.LogInformation("..finished");
            // Se retorna una respuesta con estado 'éxito'.
            yield return new DicomCMoveResponse(request, DicomStatus.Success);
        }

        /*
        Tipo: DICOM C-GET
        Recibe: DicomCGetRequest
        Responde: DicomCGetResponse
        Descripcion: Funcion para recuperar informacion del servidor de almacenamiento
        */
        public async IAsyncEnumerable<DicomCGetResponse> OnCGetRequestAsync(DicomCGetRequest request)
        {
            // Se crea un servicio de búsqueda de imágenes DICOM.
            IDicomImageFinderService finderService = QRServer.CreateFinderService;
            // Se inicializa una lista vacía de archivos correspondientes.
            IEnumerable<string> matchingFiles = Enumerable.Empty<string>();
            // Dependiendo del nivel de la solicitud, se buscan los archivos correspondientes.
            switch (request.Level)
            {
                case DicomQueryRetrieveLevel.Patient: //Paciente
                    matchingFiles = finderService.FindFilesByUID(request.Dataset.GetString(DicomTag.PatientID), string.Empty, string.Empty);
                    break;

                case DicomQueryRetrieveLevel.Study: //Estudio
                    matchingFiles = finderService.FindFilesByUID(string.Empty, request.Dataset.GetString(DicomTag.StudyInstanceUID), string.Empty);
                    break;

                case DicomQueryRetrieveLevel.Series: //Serie
                    matchingFiles = finderService.FindFilesByUID(string.Empty, string.Empty, request.Dataset.GetString(DicomTag.SeriesInstanceUID));
                    break;
                // Si el nivel es 'Image', se retorna una respuesta indicando incapacidad para realizar suboperaciones.
                case DicomQueryRetrieveLevel.Image://Imagen
                    yield return new DicomCGetResponse(request, DicomStatus.QueryRetrieveUnableToPerformSuboperations);
                    yield break;
            }

            // Se itera sobre los archivos correspondientes y se envían solicitudes de almacenamiento para cada uno.
            foreach (var matchingFile in matchingFiles)
            {
                var storeRequest = new DicomCStoreRequest(matchingFile);
                SendRequestAsync(storeRequest).Wait();
            }
            // Se retorna una respuesta con estado 'éxito'.
            yield return new DicomCGetResponse(request, DicomStatus.Success);
        }

        public async Task<DicomCStoreResponse> OnCStoreRequestAsync(DicomCStoreRequest request)
        {


            // Aquí es donde implementarías la lógica para manejar la solicitud de almacenamiento.

            // Por ejemplo, puedes obtener los datos de la instancia DICOM utilizando request.Dataset y guardarlos en tu sistema.

            // Puedes utilizar la clase DicomFile para guardar la instancia DICOM en un archivo:

            DicomFile dicomFile = new DicomFile(request.Dataset);
            //dicomFile.Dataset.Contains(DicomTag.InstitutionName)

            //** Acceso a AD de azure
            // Obtiene el token de acceso

            // Creacion de cliente Dicom web

            //Autenticacion, asigna valores de cabecera

            // ** Preparo la clase de almacenamiento


            //Task<DicomWebResponse<string>> Store(IEnumerable<DicomFile> dicomFiles, CancellationToken cancellationToken = default);

            // Después de procesar la solicitud de almacenamiento, debes devolver una respuesta con el estado adecuado.
            // Por ejemplo, si el almacenamiento se realiza correctamente, puedes devolver un estado de éxito:
            return new DicomCStoreResponse(request, DicomStatus.Success);
        }
        /*
        public async Task<Task> OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // Registra la excepción y otra información relevante
            Logger.Error($"Exception during C-STORE request. Temp file: {tempFileName}. Exception: {e}");
            DicomDataset dicomDataset = new DicomDataset();
            // Realiza cualquier acción de limpieza necesaria, como eliminar el archivo temporal
            try
            {
                // Attempt to load the temporary file into the DicomDataset
                if (File.Exists(tempFileName))
                {
                    Logger.Error($"OnCStoreRequestExceptionAsync: {tempFileName}.");

                    /*using (DicomFile dicomFile = await DicomFile.OpenAsync(tempFileName))
                    {
                        dicomDataset = dicomFile.Dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load temp file into DicomDataset. Temp file: {tempFileName}. Exception: {ex}");
            }

            // Add exception details to the DicomDataset
            dicomDataset.Add(DicomTag.ErrorComment, e.Message);
            dicomDataset.Add(DicomTag.FailureReason, e.StackTrace);

            // Perform any necessary cleanup, such as deleting the temp file
            // ...
            //ProcessingFailure
            // Notifica al cliente que se produjo una excepción devolviendo una respuesta DICOM con un estado de error
            var response = new DicomCStoreResponse(dicomDataset);
            return SendResponseAsync(response);
        }
		*/
        Task IDicomCStoreProvider.OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // Registra la excepción y otra información relevante
            Logger.LogError($"Exception during C-STORE request. Temp file: {tempFileName}. Exception: {e}");
            DicomDataset dicomDataset = new DicomDataset();
            var response = new DicomCStoreResponse(dicomDataset);
            return SendResponseAsync(response);
        }

    }
}
