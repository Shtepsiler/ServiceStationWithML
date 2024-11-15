using AutoMapper;
using JOBS.BLL.Common.Mappings;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Entities;

namespace JOBS.BLL.DTOs.Requests
{
    public class MechanicRequest : IMapFrom<Mechanic>
    {
        public Guid MechanicId { get; set; }
        public Guid SpecialisationId { get; set; }
        public string Specialisation { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Mechanic, MechanicRequest>()
                   .ForMember(dest => dest.Specialisation, opt => opt.MapFrom(src => src.Specialisation.Name))
                   .ReverseMap();
        }
    }
}
