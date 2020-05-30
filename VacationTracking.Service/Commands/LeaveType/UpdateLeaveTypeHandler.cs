using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.LeaveType
{
    public class UpdateLeaveTypeHandler : IRequestHandler<UpdateLeaveTypeCommand, LeaveTypeDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveTypeHandler(IUnitOfWork unitOfWork, ILogger<UpdateLeaveTypeHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<LeaveTypeDto> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //Domain.Models.LeaveType entity = await _unitOfWork.LeaveTypeRepository.GetAsync(request.CompanyId, request.LeaveTypeId);
            //if (entity == null)
            //    throw new ArgumentNullException(nameof(Domain.Models.LeaveType));

            //entity.ColorCode = request.Color;
            //entity.CompanyId = request.CompanyId;
            //entity.DefaultDaysPerYear = request.DefaultDaysPerYear;
            //entity.IsActive = request.IsActive;
            //entity.IsAllowNegativeBalance = request.IsAllowNegativeBalance;
            //entity.IsApproverRequired = request.IsApproverRequired;
            //entity.IsDefault = false;
            //entity.IsDeleted = false;
            //entity.IsHalfDaysActivated = request.IsHalfDaysActivated;
            //entity.IsHideLeaveTypeName = request.IsHideLeaveTypeName;
            //entity.IsReasonRequired = request.IsReasonRequired;
            //entity.IsUnlimited = request.IsUnlimited;
            //entity.TypeName = request.TypeName;

            //entity.UpdatedAt = DateTime.UtcNow;
            //entity.UpdatedBy = request.UserId;

            //using (_unitOfWork)
            //{
            //    var affectedRow = await _unitOfWork.LeaveTypeRepository.UpdateAsync(request.LeaveTypeId, entity);
            //}

            ////TODO: Fire "leaveTypeUpdated" event
            //return _mapper.Map<LeaveTypeDto>(entity);
        }
    }
}
