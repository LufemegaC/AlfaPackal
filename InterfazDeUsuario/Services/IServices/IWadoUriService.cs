namespace InterfazDeUsuario.Services.IServices
{
    public interface IWadoUriService
    {
        Task<HttpResponseMessage> GetStudyInstancesAsync(string token, string requestType, string studyUID, string seriesUID, string objectUID, string contentType = null, string charset = null, string transferSyntax = null, string anonymize = null);
        Task<HttpResponseMessage> GetInstancesByStudyUIDAsync(string token, string requestType, string studyUID);

    }
}
