using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.Vacation
{
    public class CreateVacationHandler : IRequestHandler<CreateVacationCommand, VacationDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVacationHandler(IUnitOfWork unitOfWork, ILogger<CreateVacationHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task<VacationDto> Handle(CreateVacationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //Domain.Models.Vacation vacationEntity = MapToEntity(request);

            ////TODO: Check rule for vacation

            //using (_unitOfWork)
            //{
            //    var affectedRow = await _unitOfWork.VacationRepository.InsertAsync(vacationEntity);
            //}

            ////TODO: Fire "teamCreated" event

            //return _mapper.Map<VacationDto>(vacationEntity);
        }

        private Domain.Models.Vacation MapToEntity(CreateVacationCommand request)
        {
            var vacationEntity = new Domain.Models.Vacation();
            Guid vacationId = Guid.NewGuid();
            vacationEntity.VacationId = vacationId;
            vacationEntity.UserId = request.UserId;
            vacationEntity.ApproverId = null;
            vacationEntity.LeaveTypeId = request.LeaveTypeId;
            vacationEntity.VacationStatus = VacationStatus.Pending;
            vacationEntity.StartDate = request.StartDate;
            vacationEntity.EndDate = request.EndDate;
            vacationEntity.Reason = request.Reason;
            vacationEntity.IsHalfDay = request.IsHalfDay;
            vacationEntity.CreatedAt = DateTime.Now;
            vacationEntity.CreatedBy = request.UserId;
            return vacationEntity;
        }
    }
}
