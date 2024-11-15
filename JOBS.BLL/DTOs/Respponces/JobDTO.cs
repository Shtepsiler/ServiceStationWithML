using AutoMapper;
using JOBS.BLL.Common.Mappings;
using JOBS.DAL.Entities;

namespace JOBS.BLL.DTOs.Respponces
{
    public class JobDTO : IMapFrom<Job>
    {


        public Guid Id { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid VehicleId { get; set; }
        public string? ModelName {  get; set; }
        public string? Status { get; set; }
        public Guid ClientId { get; set; }
        public Guid? MechanicId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public float? ModelConfidence { get; set; } = null;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobDTO, Job>().ReverseMap();
        }

    }
}
