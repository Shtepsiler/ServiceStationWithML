﻿using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.Jobs.Queries
{
    public class GetJobsQuery : IRequest<IEnumerable<JobDTO>>
    {
    }

    public class GetJobsQueryHendler : IRequestHandler<GetJobsQuery, IEnumerable<JobDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetJobsQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDTO>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Job>, IEnumerable<JobDTO>>(await _context.Jobs.ToListAsync());
        }
    }



}
