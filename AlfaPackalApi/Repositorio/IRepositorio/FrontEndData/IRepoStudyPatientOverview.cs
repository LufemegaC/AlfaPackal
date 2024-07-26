using AlfaPackalApi.Modelos;

namespace Api_PACsServer.Repositorio.IRepositorio.FrontEndData
{
    public interface IRepoStudyPatientOverview
    {
        IQueryable<Estudio> GetMainStudiesList(int institutionId);
    }
}
