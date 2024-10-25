using AlfaPackalApi.Datos;
using Microsoft.EntityFrameworkCore;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;


namespace Api_PACsServer.Repositorio.Pacs
{
    public class StudyRepository : ReadWriteRepository<Study>, IStudyRepository
    {
        private readonly ApplicationDbContext _db;
        private int _pageSize; // Number of records for each page

        public StudyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            _pageSize = 15; 
        }

        public PagedList<Study> GetStudies(int pageNumber, int pageSize)
        {     
            IQueryable<Study> query = _db.Studies
            //.Where(study => study.InstitutionID == parameters.InstitutionId)
            .OrderByDescending(study => study.StudyDate);

            // Get the total count of records that match the query
            var count = query.Count();
            // Apply pagination to the query
            var items = query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();
            // Return the paginated list of studies
            return new PagedList<Study>(items, count, pageNumber, pageSize);
        }
    }
}
