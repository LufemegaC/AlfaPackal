using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IEstudioRepositorio : IRepositorio<Estudio>
    {
         //Task<Estudio> Actualizar(Estudio entidad);
        Task<Estudio> GetByInstanceUID(string studyInstanceUID);
        Task<Estudio> GetByAccessionNumber(string accessionNumbers);
        Task<bool> ExistByInstanceUID(string studyInstanceUID);
        // Metodo parte del proceso C-FIND
        Task<List<Estudio>> FindByPatientIds(List<string> pacsPatientIds);
        
    }
}
