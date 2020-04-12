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
using VacationTracking.Domain.Queries.Vacation;

namespace VacationTracking.Service.Queries.Vacation
{
    public class GetVacationListHandler : IRequestHandler<GetVacationListQuery, IList<VacationDto>>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetVacationListHandler(IVacationRepository vacationRepository, IMapper mapper, ILogger<GetVacationListHandler> logger)
        {
            _vacationRepository = vacationRepository ?? throw new ArgumentNullException(nameof(vacationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<VacationDto>> Handle(GetVacationListQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var vacations = await _vacationRepository.GetListAsync(request.CompanyId);

            if (vacations != null)
            {
                _logger.LogInformation($"Got a request get vacation by CompanyId: {request.CompanyId}");
                List<VacationDto> vacationDtos = new List<VacationDto>();
                foreach (var vacation in vacations)
                {
                    vacationDtos.Add(_mapper.Map<VacationDto>(vacation));
                }
                return vacationDtos;
            }

            return null;
        }
    }
}
