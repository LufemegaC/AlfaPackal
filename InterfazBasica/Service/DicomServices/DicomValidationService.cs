using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Models.Pacs;
using InterfazBasica_DCStore.Service.IDicomService;
using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Utileria;

namespace InterfazBasica_DCStore.Service.DicomServices;
public class DicomValidationService : IDicomValidationService
{

    private readonly IGeneralAPIServices _validationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _token;

    public DicomValidationService(IGeneralAPIServices validationService, IHttpContextAccessor httpContextAccessor)
    {
        _validationService = validationService;
        _httpContextAccessor = httpContextAccessor;
    }


    //Implementacion de la interfaz
    public DicomStatus IsValidDicomFile(DicomFile dicomFile)
    // Metodo para la validacion de un archivo DICOM
    {
        // Verificación de nulidad del archivo DICOM y sus componentes esenciales
        if (dicomFile == null || dicomFile.Dataset == null || dicomFile.FileMetaInfo == null)
            return DicomStatus.InvalidObjectInstance;
        // Validacion de metadatos importantes
        // Identificador de paciente
        //if (!dicomFile.Dataset.Contains(DicomTag.PatientID))
        //    return DicomStatus.InvalidAttributeValue;
        // Identificador de estudio
        if (!dicomFile.Dataset.Contains(DicomTag.StudyInstanceUID))
            return DicomStatus.InvalidAttributeValue;
        // Verificación de la presencia y validez de UIDs
        var sopClassUid = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPClassUID, null);
        var instanceUid = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
        if (string.IsNullOrWhiteSpace(sopClassUid))
            return DicomStatus.NoSuchSOPClass;
        // Validacion de formato de UIDs
        if (!DicomUID.IsValidUid(sopClassUid) || (!DicomUID.IsValidUid(instanceUid)))
            return DicomStatus.InvalidAttributeValue;
        // Valido que se trate de un archivo DICOM de imagen 
        DicomUID uid = DicomUID.Parse(sopClassUid);
        if (!uid.IsImageStorage)
            return DicomStatus.SOPClassNotSupported;
        // Verificar la coherencia entre el Media Storage SOP Class UID y SOP Class UIDs dentro del Dataset.
        DicomUID mediaStorageSOPClassUID = dicomFile.FileMetaInfo.MediaStorageSOPClassUID;
        DicomUID sopClassUID = DicomUID.Parse(dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPClassUID, null));
        if (mediaStorageSOPClassUID != sopClassUID)
            return DicomStatus.StorageDataSetDoesNotMatchSOPClassError;
        //Paso las valdaciones
        return DicomStatus.Success;
    }

    public async Task<MainEntitiesValues> ValidateEntities(MainEntitiesValues mainEntitiesValues)
    {
        try
        {
            var response = await _validationService.ValidateEntities<APIResponse>(mainEntitiesValues,Token);
            if (response is null)
                throw new InvalidOperationException("El resultado no valido.");
            mainEntitiesValues = JsonConvert.DeserializeObject<MainEntitiesValues>(response.ResultadoJson);
            
            
            return mainEntitiesValues;
        }
        catch (Exception ex)
        {
            // Manejo de errores al intentar registrar al paciente.
            return null;
        }
    }

    public string Token
    {
        get
        {
            if (string.IsNullOrEmpty(_token))
            {
                //_token = _httpContextAccessor.HttpContext?.Session.GetString(ServerInfo.SessionToken);
                _token = ServerInfo.Token;
            }
            return _token;
        }
    }

}

