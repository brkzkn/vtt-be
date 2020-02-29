using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Holiday;

namespace VacationTracking.Service.Queries.Holiday
{
    public class GetHolidayListHandler : IRequestHandler<GetHolidayListQuery, IList<HolidayDto>>
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetHolidayListHandler(IHolidayRepository holidayRepository, IMapper mapper, ILogger<GetHolidayListHandler> logger)
        {
            _holidayRepository = holidayRepository ?? throw new ArgumentNullException(nameof(holidayRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<HolidayDto>> Handle(GetHolidayListQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var holidays = await _holidayRepository.GetListAsync(request.CompanyId);

            if (holidays != null)
            {
                _logger.LogInformation($"Got a request get holidays by CompanyId: {request.CompanyId}");
                List<HolidayDto> holidayDtos = new List<HolidayDto>();
                foreach (var holiday in holidays)
                {
                    holidayDtos.Add(_mapper.Map<HolidayDto>(holiday));
                }
                return holidayDtos;
            }

            //throw exception
            return null;
        }
    }
}
