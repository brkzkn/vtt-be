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
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using HolidayDb = VacationTracking.Domain.Models.Holiday;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Service.Commands.Holiday
{
    public class UpdateHolidayHandler : IRequestHandler<UpdateHolidayCommand, HolidayDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HolidayDb> _repository;
        private readonly IRepository<TeamDb> _teamRepository;

        public UpdateHolidayHandler(IUnitOfWork unitOfWork,
                                    IRepository<HolidayDb> repository,
                                    IRepository<TeamDb> teamRepository,
                                    ILogger<UpdateHolidayHandler> logger,
                                    IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        public async Task<HolidayDto> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Queryable()
                                          .Include(x => x.HolidayTeam)
                                          .SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.HolidayId == request.HolidayId);
            if (entity == null)
                throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Holiday cannot found with holidayId: {request.HolidayId} and companyId: {request.CompanyId}", 404);

            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsFullDay = request.IsFullDay;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = request.UserId;
            entity.Name = request.Name;

            List<int> teamIds = request.Teams as List<int>;
            if (request.IsForAllTeams)
            {
                teamIds = await _teamRepository.Queryable()
                                               .Where(x => x.CompanyId == request.CompanyId)
                                               .Select(x => x.TeamId)
                                               .ToListAsync();

            }
            var existingHolidays = _repository.Queryable().Include(x => x.HolidayTeam).Where(x => x.CompanyId == request.CompanyId);
            entity.HolidayTeam.Clear();

            foreach (var teamId in teamIds)
            {
                if (existingHolidays.Any(x => (request.StartDate <= x.EndDate && request.EndDate >= x.StartDate)
                                            && x.HolidayTeam.Any(ht => ht.TeamId == teamId)
                                            && x.HolidayId != entity.HolidayId))
                {
                    throw new VacationTrackingException(ExceptionMessages.HolidayAlreadyExistForSameDate,
                                                   $"StartDate: {request.StartDate} or" +
                                                   $"EndDate: {request.EndDate} already added for " +
                                                   $"company: {request.CompanyId} and team: {teamId}",
                                                   400);
                }

                var holidayTeam = new HolidayTeam
                {
                    TeamId = teamId,
                    HolidayId = entity.HolidayId
                };

                if (!entity.HolidayTeam.Any(x => x.TeamId == teamId))
                    entity.HolidayTeam.Add(holidayTeam);
            }

            _repository.Update(entity);
            _unitOfWork.SaveChanges();

            //TODO: Fire "holidayUpdated" event
            return _mapper.Map<HolidayDto>(entity);
        }
    }
}
