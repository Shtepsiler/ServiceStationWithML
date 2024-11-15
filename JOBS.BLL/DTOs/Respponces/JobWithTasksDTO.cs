using AutoMapper;
using JOBS.BLL.Common.Mappings;
using JOBS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.DTOs.Respponces
{
    public class JobWithTasksDTO : IMapFrom<JobWithTasksDTO>
    {


        public Guid Id { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid VehicleId { get; set; }
        public string? ModelName { get; set; }
        public string? Status { get; set; }
        public Guid ClientId { get; set; }
        public Guid? MechanicId { get; set; }
        public Guid? OrderId { get; set; }
        public string? Specialisation { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public float? ModelConfidence { get; set; } = null;

        public List<MechanicsTasksDTO> Tasks { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobWithTasksDTO, Job>().ReverseMap().ForMember(p=>p.Tasks,map=>map.MapFrom(p=>p.Tasks)).ReverseMap();
        }

    }
}
