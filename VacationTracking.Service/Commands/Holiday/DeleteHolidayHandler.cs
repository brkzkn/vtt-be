using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using HolidayDb = VacationTracking.Domain.Models.Holiday;

namespace VacationTracking.Service.Commands.Holiday
{
    public class DeleteHolidayHandler : IRequestHandler<DeleteHolidayCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HolidayDb> _repository;

        public DeleteHolidayHandler(IUnitOfWork unitOfWork,
                                    IRepository<HolidayDb> repository,
                                    ILogger<DeleteHolidayHandler> logger,
                                    IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Queryable()
                                          .Include(x => x.HolidayTeam)
                                          .SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.HolidayId == request.HolidayId);

            if (entity == null)
                throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Holiday not found. HolidayId: {request.HolidayId}", 404);

            _repository.Delete(entity);
            int affectedRows = _unitOfWork.SaveChanges();
            return affectedRows > 0;
        }
    }
}
