using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.LeaveType;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Service.Queries.LeaveType
{
    public class GetLeaveTypeListHandler : IRequestHandler<GetLeaveTypeListQuery, IList<LeaveTypeDto>>
    {
        private readonly IRepository<LeaveTypeDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetLeaveTypeListHandler(IRepository<LeaveTypeDb> repository, IMapper mapper, ILogger<GetLeaveTypeListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<LeaveTypeDto>> Handle(GetLeaveTypeListQuery request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _repository.Queryable()
                                              .Where(x => x.CompanyId == request.CompanyId && x.IsDeleted == false)
                                              .ToListAsync();

            List<LeaveTypeDto> leaveTypeDtos = new List<LeaveTypeDto>();

            if (leaveTypes != null)
            {
                _logger.LogInformation($"Got a request get leaveType by CompanyId: {request.CompanyId}");
                foreach (var leaveType in leaveTypes)
                {
                    leaveTypeDtos.Add(_mapper.Map<LeaveTypeDto>(leaveType));
                }
            }

            return leaveTypeDtos;
        }
    }
}
