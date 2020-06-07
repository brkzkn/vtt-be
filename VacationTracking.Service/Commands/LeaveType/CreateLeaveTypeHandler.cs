using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
    public class CreateLeaveTypeHandler : IRequestHandler<CreateLeaveTypeCommand, LeaveTypeDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<LeaveTypeDb> _repository;
        public CreateLeaveTypeHandler(IUnitOfWork unitOfWork, IRepository<LeaveTypeDb> repository, ILogger<CreateLeaveTypeHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LeaveTypeDto> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            bool isNameUnique = await _repository.IsLeaveTypeNameExistAsync(request.CompanyId, request.LeaveTypeName);
            if (isNameUnique)
                throw new VacationTrackingException(ExceptionMessages.LeaveTypeNameAlreadyExist, $"Leave type name: {request.LeaveTypeName} already exist", 400);

            var entity = new LeaveTypeDb()
            {
                ColorCode = request.ColorCode,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.Now,
                CreatedBy = request.UserId,
                DefaultDaysPerYear = request.DefaultDaysPerYear,
                IsActive = true,
                IsAllowNegativeBalance = request.IsAllowNegativeBalance,
                IsApproverRequired = request.IsApproverRequired,
                IsDefault = false,
                IsDeleted = false,
                IsHalfDaysActivated = request.IsHalfDaysActivated,
                IsHideLeaveTypeName = request.IsHideLeaveTypeName,
                IsReasonRequired = request.IsReasonRequired,
                IsUnlimited = request.IsUnlimited,
                LeaveTypeName = request.LeaveTypeName
            };

            _repository.Insert(entity);
            int affectedRows = await _unitOfWork.SaveChangesAsync();

            //TODO: Fire "leaveTypeCreated" event
            return _mapper.Map<LeaveTypeDto>(entity);
        }
    }
}
