using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Repositorio.IRepositorio;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface IEstudioRepositorio : IRepositorioEscritura<Estudio>, IRepositorioLectura<Estudio>
    {
        Task<Estudio> GetByInstanceUID(string studyInstanceUID);
        Task<Estudio> GetByAccessionNumber(string accessionNumbers);
        Task<bool> ExistByInstanceUID(string studyInstanceUID);
        // Metodo parte del proceso C-FIND
        Task<List<Estudio>> FindByPatientIds(List<string> pacsPatientIds);
    }
}
