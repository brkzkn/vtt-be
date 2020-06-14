using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.Repository.LeaveType;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Service.Commands.LeaveType
{
    public class UpdateLeaveTypeHandler : IRequestHandler<UpdateLeaveTypeCommand, LeaveTypeDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<LeaveTypeDb> _repository;

        public UpdateLeaveTypeHandler(IUnitOfWork unitOfWork,
                                      IRepository<LeaveTypeDb> repository,
                                      ILogger<UpdateLeaveTypeHandler> logger,
                                      IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LeaveTypeDto> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Queryable()
                                          .SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId
                                                                  && x.LeaveTypeId == request.LeaveTypeId);
            if (entity == null)
                throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"LeaveType not found. LeaveTypeId: {request.LeaveTypeId}", 404);

            bool isNameUnique = await _repository.IsLeaveTypeNameExistAsync(request.CompanyId, request.LeaveTypeName);
            if (isNameUnique)
                throw new VacationTrackingException(ExceptionMessages.LeaveTypeNameAlreadyExist, $"Leave type name: {request.LeaveTypeName} already exist", 400);

            entity.ColorCode = request.ColorCode;
            entity.CompanyId = request.CompanyId;
            entity.DefaultDaysPerYear = request.DefaultDaysPerYear;
            entity.IsActive = request.IsActive;
            entity.IsAllowNegativeBalance = request.IsAllowNegativeBalance;
            entity.IsApproverRequired = request.IsApproverRequired;
            entity.IsDefault = false;
            entity.IsDeleted = false;
            entity.IsHalfDaysActivated = request.IsHalfDaysActivated;
            entity.IsHideLeaveTypeName = request.IsHideLeaveTypeName;
            entity.IsReasonRequired = request.IsReasonRequired;
            entity.IsUnlimited = request.IsUnlimited;
            entity.LeaveTypeName = request.LeaveTypeName;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = request.UserId;

            _repository.Update(entity);
            int affectedRow = await _unitOfWork.SaveChangesAsync();

            //TODO: Fire "leaveTypeUpdated" event
            return _mapper.Map<LeaveTypeDto>(entity);
        }
    }
}
