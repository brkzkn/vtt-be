using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Queries.Holiday;
using HolidayDb = VacationTracking.Domain.Models.Holiday;

namespace VacationTracking.Service.Queries.Holiday
{
    public class GetHolidayHandler : IRequestHandler<GetHolidayQuery, HolidayDto>
    {
        private readonly IRepository<HolidayDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetHolidayHandler(IRepository<HolidayDb> repository, IMapper mapper, ILogger<GetHolidayHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HolidayDto> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var holiday = await _repository.Queryable()
                                           .SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId
                                                                   && x.HolidayId == request.HolidayId);

            if (holiday != null)
            {
                _logger.LogInformation($"Got a request get holiday Id: {holiday.HolidayId}");
                var holidayDto = _mapper.Map<HolidayDto>(holiday);
                return holidayDto;
            }

            throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Holiday not found by id {request.HolidayId}", 404);
        }
    }
}
