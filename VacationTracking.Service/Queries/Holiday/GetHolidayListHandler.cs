using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Holiday;
using HolidayDb = VacationTracking.Domain.Models.Holiday;

namespace VacationTracking.Service.Queries.Holiday
{
    public class GetHolidayListHandler : IRequestHandler<GetHolidayListQuery, IEnumerable<HolidayDto>>
    {
        private readonly IRepository<HolidayDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetHolidayListHandler(IRepository<HolidayDb> repository, IMapper mapper, ILogger<GetHolidayListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IEnumerable<HolidayDto>> Handle(GetHolidayListQuery request, CancellationToken cancellationToken)
        {
            var holidays = await _repository.Queryable().Where(x => x.CompanyId == request.CompanyId).ToListAsync();

            _logger.LogInformation($"Got a request get holidays by CompanyId: {request.CompanyId}");

            List<HolidayDto> holidayDtos = new List<HolidayDto>();

            if (holidays.Count > 0)
            {
                holidayDtos.AddRange(holidays.Select(x => _mapper.Map<HolidayDto>(x)).ToList());
            }

            return holidayDtos;
        }
    }
}
