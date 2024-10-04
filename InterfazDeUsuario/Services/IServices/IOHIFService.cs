using InterfazDeUsuario.Models.visorOHIF;

namespace InterfazDeUsuario.Services.IServices
{
    public interface IOHIFService
    {
        Task SendStudyDataAsync(OHIFStudy studyInfo);
    }
}
