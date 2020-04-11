using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.LeaveType
{
    public class CreateLeaveTypeHandler : IRequestHandler<CreateLeaveTypeCommand, LeaveTypeDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveTypeHandler(IUnitOfWork unitOfWork, ILogger<CreateLeaveTypeHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<LeaveTypeDto> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.LeaveType entity = MapToEntity(request);

            using (_unitOfWork)
            {
                var isTypeNameValid = await _unitOfWork.LeaveTypeRepository.IsLeaveTypeExistAsync(entity.CompanyId, entity.TypeName);
                if (isTypeNameValid)
                    throw new Exception("NameAlreadyExist"); //TODO: Replace custom exception handler

                var affectedRow = await _unitOfWork.LeaveTypeRepository.InsertAsync(entity);
            }

            //TODO: Fire "leaveTypeCreated" event
            return _mapper.Map<LeaveTypeDto>(entity);
        }

        private Domain.Models.LeaveType MapToEntity(CreateLeaveTypeCommand request)
        {
            var entity = new Domain.Models.LeaveType();
            Guid leaveTypeId = Guid.NewGuid();

            entity.ColorCode = request.Color;
            entity.CompanyId = request.CompanyId;

            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = request.UserId;
            entity.DefaultDaysPerYear = request.DefaultDaysPerYear;
            entity.IsActive = true;
            entity.IsAllowNegativeBalance = request.IsAllowNegativeBalance;
            entity.IsApproverRequired = request.IsApproverRequired;
            entity.IsDefault = false;
            entity.IsDeleted = false;
            entity.IsHalfDaysActivated = request.IsHalfDaysActivated;
            entity.IsHideLeaveTypeName = request.IsHideLeaveTypeName;
            entity.IsReasonRequired = request.IsReasonRequired;
            entity.IsUnlimited = request.IsUnlimited;
            entity.LeaveTypeId = leaveTypeId;
            entity.TypeName = request.TypeName;
            return entity;
        }
    }
}
