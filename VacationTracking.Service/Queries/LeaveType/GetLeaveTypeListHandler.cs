using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.LeaveType;

namespace VacationTracking.Service.Queries.LeaveType
{
    public class GetLeaveTypeListHandler : IRequestHandler<GetLeaveTypeListQuery, IList<LeaveTypeDto>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetLeaveTypeListHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper, ILogger<GetLeaveTypeListHandler> logger)
        {
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<LeaveTypeDto>> Handle(GetLeaveTypeListQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var leaveTypes = await _leaveTypeRepository.GetListAsync(request.CompanyId);

            if (leaveTypes != null)
            {
                _logger.LogInformation($"Got a request get leaveType by CompanyId: {request.CompanyId}");
                List<LeaveTypeDto> leaveTypeDtos = new List<LeaveTypeDto>();
                foreach (var leaveType in leaveTypes)
                {
                    leaveTypeDtos.Add(_mapper.Map<LeaveTypeDto>(leaveType));
                }
                return leaveTypeDtos;
            }

            //throw exception
            return null;
        }
    }
}
