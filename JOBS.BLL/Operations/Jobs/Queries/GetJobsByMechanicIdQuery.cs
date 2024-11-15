using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using JOBS.DAL.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace JOBS.BLL.Operations.Jobs.Queries
{
    public class GetJobsByMechanicIdQuery : IRequest<IEnumerable<JobDTO>>
    {
        public Guid MecchanicId { get; set; }
    }

    public class GetJobsByMechanicIdQueryHendler : IRequestHandler<GetJobsByMechanicIdQuery, IEnumerable<JobDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetJobsByMechanicIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDTO>> Handle(GetJobsByMechanicIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Job>, IEnumerable<JobDTO>>(await _context.Jobs.Where(p=>p.MechanicId == request.MecchanicId).ToListAsync());
        }
    }
}


