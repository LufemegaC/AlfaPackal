using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.Studies;

namespace Api_PACsServer.Models.Dto
{
    public class MainEntitiesCreateDto
    {
        public StudyCreateDto StudyCreate { get; set; }
        public SerieCreateDto SerieCreate { get; set; }
        public InstanceCreateDto InstanceCreate { get; set; }

        
    }
}
