using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Holiday;

namespace VacationTracking.Service.Queries.Holiday
{
    public class GetHolidayHandler : IRequestHandler<GetHolidayQuery, HolidayDto>
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetHolidayHandler(IHolidayRepository teamRepository, IMapper mapper, ILogger<GetHolidayHandler> logger)
        {
            _holidayRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HolidayDto> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var holiday = await _holidayRepository.GetAsync(request.CompanyId, request.HolidayId);

            if (holiday != null)
            {
                _logger.LogInformation($"Got a request get holiday Id: {holiday.HolidayId}");
                var teamDto = _mapper.Map<HolidayDto>(holiday);
                return teamDto;
            }

            // TODO: throw exception not found
            return null;
        }

    }
}
