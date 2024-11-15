using AutoMapper;
using JOBS.BLL.Common.Mappings;
using JOBS.DAL.Entities;

namespace JOBS.BLL.DTOs.Respponces
{
    public class MechanicsTasksDTO : IMapFrom<MechanicsTasks>
    {
        public Guid Id { get; set; }
        public Guid MechanicId { get; set; }
        public string? Name { get; set; }
        public Guid? JobId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? Task { get; set; }
        public string? Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MechanicsTasksDTO, MechanicsTasks>().ReverseMap();
        }

    }

}
