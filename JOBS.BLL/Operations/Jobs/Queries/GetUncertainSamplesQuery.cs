using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.Operations.Jobs.Queries
{
    public class GetUncertainSamplesQuery : IRequest<RetrainRespponce?>
    {
        public float Confidence { get; set; } = 0.5f;
        public bool ChoseApproverd { get; set; } = false;

    }
    public class GetUncertainSamplesQueryHendler : IRequestHandler<GetUncertainSamplesQuery, RetrainRespponce?>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper mapper;

        public GetUncertainSamplesQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<RetrainRespponce?> Handle(GetUncertainSamplesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Створення базового запиту
                var uncertainJobsQuery = _context.Jobs.Include(p=>p.Mechanic).ThenInclude(p=>p.Specialisation).ToList();
                var uncertainJobsQuer = uncertainJobsQuery.Where(job => job.ModelConfidence.HasValue && job.ModelConfidence <= request.Confidence);

                // Якщо ChoseApproverd == false або не вказано, додаємо фільтр по ModelAproved
                if (!request.ChoseApproverd)
                {
                    uncertainJobsQuer = uncertainJobsQuer.Where(job => job.ModelAproved == false);
                }

                // Виконання запиту
                var uncertainJobs = uncertainJobsQuer
                    .Select(job => new
                    {
                        Id = job.Id,
                        Data = job.Description ?? string.Empty,  // Дані для тренування
                        Label = job.Mechanic.Specialisation.Name ?? "Unknown"  // Мітки для тренування
                    })
                    .ToList();

                // Якщо немає даних, повернути null
                if (!uncertainJobs.Any())
                    return null;

                // Формування результату
                var retrainRequest = new RetrainRespponce
                {
                    ids = uncertainJobs.Select(p => p.Id).ToList(),
                    new_data = uncertainJobs.Select(j => j.Data).ToList(),
                    new_labels = uncertainJobs.Select(j => j.Label).ToList()
                };

                return retrainRequest;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving uncertain samples", ex);
            }
        }


    }
}
