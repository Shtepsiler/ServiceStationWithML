using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using JOBS.DAL.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.Jobs.Queries
{
    public class GetJobByIdQuery : IRequest<JobWithTasksDTO?>
    {
        public Guid Id { get; set; }

    }
    public class GetJobByIdQueryHendler : IRequestHandler<GetJobByIdQuery, JobWithTasksDTO?>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper mapper;

        public GetJobByIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<JobWithTasksDTO?> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Завантажуємо тільки один запис із пов’язаними даними
                var job = await _context.Jobs
                    .Include(j => j.Tasks)
                    .Include(j => j.Mechanic)
                        .ThenInclude(m => m.Specialisation)
                    .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

                // Перевіряємо, чи знайдений запис
                if (job == null)
                    throw new NotFoundException($"Job with ID {request.Id} not found");

                // Мапимо знайдену сутність на DTO
                var dto = mapper.Map<JobWithTasksDTO>(job);
                dto.Specialisation = job.Mechanic?.Specialisation?.Name;

                return dto;
            }
            catch (Exception)
            {
                // Зберігаємо оригінальний stack trace
                throw;
            }
        }

    }
}
