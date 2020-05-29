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
    public class CreateUserVacationHandler : IRequestHandler<CreateUserVacationCommand, VacationDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserVacationHandler(IUnitOfWork unitOfWork, ILogger<CreateUserVacationHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<VacationDto> Handle(CreateUserVacationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //Domain.Models.Vacation vacationEntity = MapToEntity(request);

            ////TODO: Check rule for vacation
            ///// IsUserId matched company_id
            //if (request.UserId == Guid.Empty)
            //    throw new ArgumentNullException(nameof(request.UserId), "UserId cannot be empty");

            //using (_unitOfWork)
            //{
            //    var affectedRow = await _unitOfWork.VacationRepository.InsertAsync(vacationEntity);
            //}

            ////TODO: Fire "vacationCreated" event

            //return _mapper.Map<VacationDto>(vacationEntity);
        }

        private Domain.Models.Vacation MapToEntity(CreateUserVacationCommand request)
        {
            throw new NotImplementedException();
            //var vacationEntity = new Domain.Models.Vacation();

            //vacationEntity.VacationId = vacationId;
            //vacationEntity.UserId = request.UserId;
            //vacationEntity.ApproverId = null;
            //vacationEntity.LeaveTypeId = request.LeaveTypeId;
            //vacationEntity.VacationStatus = VacationStatus.Approved;
            //vacationEntity.StartDate = request.StartDate;
            //vacationEntity.EndDate = request.EndDate;
            //vacationEntity.Reason = request.Reason;
            //vacationEntity.CreatedAt = DateTime.Now;
            //vacationEntity.CreatedBy = request.UserId;
            //return vacationEntity;
        }
    }
}
