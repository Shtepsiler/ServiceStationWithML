using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.Jobs.Queries
{
    public class GetJobsByIssueDateQuery : IRequest<IEnumerable<JobDTO>>
    {
        public DateTime IssueDate { get; set; }
    }
    public class GetJobsByIssueDateHendler : IRequestHandler<GetJobsByIssueDateQuery, IEnumerable<JobDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetJobsByIssueDateHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDTO>> Handle(GetJobsByIssueDateQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Job>, IEnumerable<JobDTO>>(await _context.Jobs.Where(p => p.IssueDate.Date == request.IssueDate.Date).ToListAsync());
        }
    }
}
