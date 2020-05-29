using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Constants;

namespace VacationTracking.Service.Commands.Vacation
{
    public class UpdateVacationHandler : IRequestHandler<UpdateVacationCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVacationHandler(IUnitOfWork unitOfWork, ILogger<CreateVacationHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task<bool> Handle(UpdateVacationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //Domain.Models.Vacation vacationEntity = MapToEntity(request);

            ////TODO: Check rule for vacation

            //int affectedRow = 0;
            //using (_unitOfWork)
            //{
            //    affectedRow = await _unitOfWork.VacationRepository.UpdateStatusAsync(vacationEntity);
            //}

            ////TODO: Fire "vacationStatusUpdated" event

            //return affectedRow != 0;
        }

        private Domain.Models.Vacation MapToEntity(UpdateVacationCommand request)
        {
            var vacationEntity = new Domain.Models.Vacation();
            vacationEntity.VacationId = request.VacationId;
            vacationEntity.ApproverId = request.ResponsedBy;
            vacationEntity.Note = request.Note;
            vacationEntity.ModifiedAt= DateTime.Now;
            vacationEntity.ModifiedBy = request.ResponsedBy;

            switch (request.Status)
            {
                case VacationStatus.Approved:
                    vacationEntity.VacationStatus = VacationStatus.Approved;
                    break;
                case VacationStatus.Rejected:
                    vacationEntity.VacationStatus = VacationStatus.Rejected;
                    break;
                default:
                    throw new Exception("Invalid vacation status");
            }
            return vacationEntity;
        }
    }
}
