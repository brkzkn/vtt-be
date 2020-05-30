using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.LeaveType;

namespace VacationTracking.Service.Queries.LeaveType
{
    public class GetLeaveTypeHandler : IRequestHandler<GetLeaveTypeQuery, LeaveTypeDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetLeaveTypeHandler(IMapper mapper, ILogger<GetLeaveTypeHandler> logger)
        {
            _leaveTypeRepository = null; // leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LeaveTypeDto> Handle(GetLeaveTypeQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var leaveType = await _leaveTypeRepository.GetAsync(request.CompanyId, request.LeaveTypeId);

            if (leaveType != null)
            {
                _logger.LogInformation($"Got a request get leaveTypeId: {leaveType.LeaveTypeId}");
                var leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveType);
                return leaveTypeDto;
            }

            // TODO: throw exception not found
            return null;
        }

    }
}
