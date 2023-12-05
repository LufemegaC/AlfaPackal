using AlfaPackalApi.Modelos;


namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IEstudioRepositorio : IRepositorio<Estudio>
    {
        Task<Estudio> Actualizar(Estudio entidad);
        Task<bool> ExisteStudyInstanceUID(string studyInstanceUID);
        Task<Estudio> GetStudyByInstanceUID(string studyInstanceUID);
    }
}
