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
    public class GetJobsByUserIdQuery : IRequest<IEnumerable<JobDTO>>
    {
        public Guid UserId { get; set; }
    }

    public class GetJobsByUserIdQueryHendler : IRequestHandler<GetJobsByUserIdQuery, IEnumerable<JobDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetJobsByUserIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDTO>> Handle(GetJobsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Job>, IEnumerable<JobDTO>>(await _context.Jobs.Where(p=>p.ClientId == request.UserId).ToListAsync());
        }
    }
}

