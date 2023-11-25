using AlfaPackalApi.Modelos;


namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IEstudioRespositorio : IRepositorio<Estudio>
    {
        Task<Estudio> Actualizar(Estudio entidad);
        Task<bool> ExisteStudyInstanceUID(string studyInstanceUID);
    }
}
