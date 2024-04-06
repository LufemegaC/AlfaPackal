using AlfaPackalApi.Modelos;


namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IEstudioRepositorio : IRepositorio<Estudio>
    {
         //Task<Estudio> Actualizar(Estudio entidad);
        Task<Estudio> GetByInstanceUID(string studyInstanceUID);
        Task<Estudio> GetByAccessionNumber(string accessionNumbers);
        Task<bool> ExistByInstanceUID(string studyInstanceUID);
    }
}
