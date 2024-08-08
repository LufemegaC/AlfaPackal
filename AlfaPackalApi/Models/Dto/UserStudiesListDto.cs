using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto.Studies;

namespace Api_PACsServer.Models.Dto
{
    public class UserStudiesListDto
    {
        public IEnumerable<StudyDto> Studies { get; set; }
        public int TotalPages { get; set; }

    }

}
