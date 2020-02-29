using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Domain.Commands.Holiday;

namespace VacationTracking.Service.Commands.Holiday
{
    public class DeleteHolidayHandler : IRequestHandler<DeleteHolidayCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHolidayHandler(IUnitOfWork unitOfWork, ILogger<DeleteHolidayHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Begin();
                var affectedRows = await _unitOfWork.HolidayRepository.RemoveTeamHolidays(request.HolidayId);
                affectedRows = await _unitOfWork.HolidayRepository.RemoveAsync(request.HolidayId, request.CompanyId);

                _unitOfWork.Commit();
                return affectedRows > 0;
            }
        }
    }
}
