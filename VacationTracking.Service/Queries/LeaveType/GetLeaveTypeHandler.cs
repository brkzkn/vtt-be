using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Queries.LeaveType;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Service.Queries.LeaveType
{
    public class GetLeaveTypeHandler : IRequestHandler<GetLeaveTypeQuery, LeaveTypeDto>
    {
        private readonly IRepository<LeaveTypeDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetLeaveTypeHandler(IRepository<LeaveTypeDb> repository, IMapper mapper, ILogger<GetLeaveTypeHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LeaveTypeDto> Handle(GetLeaveTypeQuery request, CancellationToken cancellationToken)
        {
            var leaveType = await _repository.Queryable().SingleOrDefaultAsync(x => x.LeaveTypeId == request.LeaveTypeId
                                                                                 && x.CompanyId == request.CompanyId
                                                                                 && x.IsDeleted == false);

            if (leaveType != null)
            {
                _logger.LogInformation($"Got a request get leaveTypeId: {leaveType.LeaveTypeId}");
                var leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveType);
                return leaveTypeDto;
            }

            throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"LeaveType not found by id {request.LeaveTypeId}", 404);
        }

    }
}
