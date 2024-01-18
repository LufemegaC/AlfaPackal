using AutoMapper;
using FellowOakDicom;
using InterfazBasica.Models.Pacs;

namespace InterfazBasica_DCStore
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            //Paciente 
            CreateMap<PacienteDto, PacienteCreateDto>().ReverseMap();
            CreateMap<PacienteDto, PacienteUpdateDto>().ReverseMap();
            //Estudio
            CreateMap<EstudioDto, EstudioCreateDto>().ReverseMap();
            CreateMap<EstudioDto, EstudioUpdateDto>().ReverseMap();
            //Serie
            CreateMap<SerieDto, SerieCreateDto>().ReverseMap();
            CreateMap<SerieDto, SerieUpdateDto>().ReverseMap();
            //Imagen
            CreateMap<ImagenDto, ImagenCreateDto>().ReverseMap();
            CreateMap<ImagenDto, ImagenUpdateDto>().ReverseMap();

            //-- Mapeo de DICOM a entidades PACS
            // Paciente
            // Mapeo de PacienteCreateDto a DicomDataset
            CreateMap<PacienteCreateDto, DicomDataset>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => MapToDicomDataset(src)));
            // Estudio
            CreateMap<EstudioCreateDto, DicomDataset>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => MapToDicomDataset(src)));
            //Serie
            CreateMap<SerieCreateDto, DicomDataset>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => MapToDicomDataset(src)));
            //Imagen
            // Mapeo de ImagenCreateDto a DicomDataset
            CreateMap<ImagenCreateDto, DicomDataset>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => MapToDicomDataset(src)));


        }

        // 18/01/2024 Luis Felipe MG: Mapeo de clase EstudioCreate con Metadatos del DataSet
        private static DicomDataset MapToDicomDataset(EstudioCreateDto src)
        {
            var dataset = new DicomDataset();

            dataset.AddOrUpdate(DicomTag.StudyInstanceUID, src.StudyInstanceUID);
            dataset.AddOrUpdate(DicomTag.PatientID, src.PACS_PatientID?.ToString());
            //dataset.AddOrUpdate(DicomTag.StudyComments, src.StudyComments); //Retired
            dataset.AddOrUpdate(DicomTag.Modality, src.Modality);
            dataset.AddOrUpdate(DicomTag.StudyDescription, src.StudyDescription);
            dataset.AddOrUpdate(DicomTag.StudyDate, src.StudyDate);
            dataset.AddOrUpdate(DicomTag.BodyPartExamined, src.BodyPartExamined);
            dataset.AddOrUpdate(DicomTag.AccessionNumber, src.AccessionNumber);
            dataset.AddOrUpdate(DicomTag.InstitutionName, src.InstitutionName);
            dataset.AddOrUpdate(DicomTag.PerformingPhysicianName, src.PerformingPhysicianName);
            dataset.AddOrUpdate(DicomTag.OperatorsName, src.OperatorName);
            return dataset;
        }
        private static DicomDataset MapToDicomDataset(SerieCreateDto src)
        {
            var dataset = new DicomDataset();
            dataset.AddOrUpdate(DicomTag.SeriesInstanceUID, src.SeriesInstanceUID);
            dataset.AddOrUpdate(DicomTag.SeriesDescription, src.SeriesDescription);
            dataset.AddOrUpdate(DicomTag.SeriesNumber, src.SeriesNumber.ToString());

            return dataset;
        }

        private static DicomDataset MapToDicomDataset(PacienteCreateDto src)
        {
            var dataset = new DicomDataset();
            // Mapeo de las propiedades de PacienteCreateDto a los tags DICOM correspondientes
            dataset.AddOrUpdate(DicomTag.PatientID, src.PatientID.ToString());
            dataset.AddOrUpdate(DicomTag.PatientName, src.PatientName);
            dataset.AddOrUpdate(DicomTag.PatientAge, src.PatientAge);
            dataset.AddOrUpdate(DicomTag.PatientSex, src.PatientSex);
            dataset.AddOrUpdate(DicomTag.PatientWeight, src.PatientWeight);
            dataset.AddOrUpdate(DicomTag.PatientBirthDate, src.PatientBirthDate);
            return dataset;
        }

        private static DicomDataset MapToDicomDataset(ImagenCreateDto src)
        {
            var dataset = new DicomDataset();
            dataset.AddOrUpdate(DicomTag.SOPInstanceUID, src.SOPInstanceUID);
            dataset.AddOrUpdate(DicomTag.ImageComments, src.ImageComments);
            dataset.AddOrUpdate(DicomTag.InstanceNumber, src.ImageNumber.ToString());
           
            return dataset;
        }

    }
}
