using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Dtos;
using VacationDb = VacationTracking.Domain.Models.Vacation;
using HolidayDb = VacationTracking.Domain.Models.Holiday;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;
using VacationTracking.Data.Repository.Holiday;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Constants;
using VacationTracking.Data.Repository.Vacation;
using VacationTracking.Data.Repository.LeaveType;

namespace VacationTracking.Service.Commands.Vacation
{
    public class CreateVacationHandler : IRequestHandler<CreateVacationCommand, VacationDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<VacationDb> _repository;
        private readonly IRepository<HolidayDb> _holidayRepository;
        private readonly IRepository<LeaveTypeDb> _leaveTypeRepository;

        public CreateVacationHandler(IUnitOfWork unitOfWork,
                                     IRepository<VacationDb> repository,
                                     IRepository<HolidayDb> holidayRepository,
                                     IRepository<LeaveTypeDb> leaveTypeRepository,
                                     ILogger<CreateVacationHandler> logger,
                                     IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _holidayRepository = holidayRepository ?? throw new ArgumentNullException(nameof(holidayRepository));
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
        }


        public async Task<VacationDto> Handle(CreateVacationCommand request, CancellationToken cancellationToken)
        {
            var entity = new VacationDb()
            {
                CreatedBy = request.UserId,
                CreatedAt = DateTime.UtcNow,
                EndDate = request.EndDate,
                IsHalfDay = request.IsHalfDay,
                Reason = request.Reason,
                StartDate = request.StartDate,
                UserId = request.UserId,
                LeaveTypeId = request.LeaveTypeId,
                VacationStatus = Domain.Enums.VacationStatus.Pending
            };

            var leaveType = await _leaveTypeRepository.GetActiveLeaveTypeAsync(request.CompanyId, request.LeaveTypeId);

            ////TODO: Check rule for vacation
            if (leaveType == null)
                throw new VacationTrackingException(ExceptionMessages.VacationLeaveTypeIsNotValid, $"Requested leave type id: ({request.LeaveTypeId}) is not valid or inactive", 400);

            if (leaveType.IsReasonRequired && string.IsNullOrEmpty(request.Reason))
                throw new VacationTrackingException(ExceptionMessages.VacationReasonIsRequired, $"Reason is required for leave type: ({leaveType.LeaveTypeName})", 400);

            if (!leaveType.IsHalfDaysActivated && request.IsHalfDay)
                throw new VacationTrackingException(ExceptionMessages.LeaveTypeDoesNotAllowHalfDays, $"Requested leave type: ({leaveType.LeaveTypeName}) doesn't allow to use half day vacation", 400);

            if (await _holidayRepository.IsDateOverlapHolidaysAsync(request.CompanyId, request.StartDate, request.EndDate))
                throw new VacationTrackingException(ExceptionMessages.VacationDateIsNotValid, "Requested date(s) are overlap with holidays", 400);

            if (await _repository.IsDateOverlapExistingVacationAsync(request.UserId, request.StartDate, request.EndDate, request.IsHalfDay))
                throw new VacationTrackingException(ExceptionMessages.VacationDateIsNotValid, "You already requested date(s) with existing vacations", 400);

            if (!leaveType.IsApproverRequired)
                entity.VacationStatus = Domain.Enums.VacationStatus.Approved;

            _repository.Insert(entity);
            var affectedRow = await _unitOfWork.SaveChangesAsync();


            ////TODO: Fire "teamCreated" event
            return _mapper.Map<VacationDto>(entity);
        }
    }
}
