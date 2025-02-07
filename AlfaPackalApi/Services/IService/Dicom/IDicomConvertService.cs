using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;

namespace Api_PACsServer.Services.IService.Dicom
{
    public interface IDicomConvertService
    {
        Task<StowRsRequestDto> ParseStowRsRequestToDto(HttpRequest request);

        Task<string> ParseDicomResultToDicomJson(List<StowInstanceResult> operationResults, string transactionUID);

    }
}
