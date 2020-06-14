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

namespace VacationTracking.Service.Commands.Vacation
{
    public class CreateVacationHandler : IRequestHandler<CreateVacationCommand, VacationDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<VacationDb> _repository;

        public CreateVacationHandler(IUnitOfWork unitOfWork,
                                     IRepository<VacationDb> repository,
                                     ILogger<CreateVacationHandler> logger,
                                     IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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

            ////TODO: Check rule for vacation


            _repository.Insert(entity);
            var affectedRow = await _unitOfWork.SaveChangesAsync();


            ////TODO: Fire "teamCreated" event
            return _mapper.Map<VacationDto>(entity);
        }
    }
}
