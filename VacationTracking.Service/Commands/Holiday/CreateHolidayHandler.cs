﻿using AutoMapper;
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
    public class CreateHolidayHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HolidayDb> _repository;
        private readonly IRepository<TeamDb> _teamRepository;
        public CreateHolidayHandler(IUnitOfWork unitOfWork, IRepository<HolidayDb> repository, IRepository<TeamDb> teamRepository,
            ILogger<CreateHolidayHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
        {
            var entity = new HolidayDb()
            {
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId,
                EndDate = request.EndDate,
                Name = request.Name,
                StartDate = request.StartDate,
                HolidayTeam = new List<HolidayTeam>()
            };

            List<int> teamIds = request.Teams as List<int>;
            if (request.IsForAllTeams)
            {
                teamIds = await _teamRepository.Queryable()
                                               .Where(x => x.CompanyId == request.CompanyId)
                                               .Select(x => x.TeamId)
                                               .ToListAsync();

            }
            var existingHolidays = _repository.Queryable().Include(x => x.HolidayTeam).Where(x => x.CompanyId == request.CompanyId);
            
            foreach (var teamId in teamIds)
            {
                if (existingHolidays.Any(x => (x.StartDate == request.StartDate || x.EndDate == request.EndDate)
                                            && x.HolidayTeam.Any(ht => ht.TeamId == teamId)))
                {
                    throw new VacationTrackingException(ExceptionMessages.HolidayAlreadyExistForSameDate,
                                                   $"StartDate: {request.StartDate} or" +
                                                   $"EndDate: {request.EndDate} already added for " +
                                                   $"company: {request.CompanyId} and team: {teamId}",
                                                   400);
                }

                entity.HolidayTeam.Add(new Domain.Models.HolidayTeam
                {
                    TeamId = teamId
                });
            }

            _repository.Attach(entity);
            _unitOfWork.SaveChanges();

            //using (_unitOfWork)
            //{
            //    _unitOfWork.Begin();

            //    /* TODO: Check holiday date; 
            //     * 1. if date is already exist for team or general,
            //     */
            //    var affectedRow = await _unitOfWork.HolidayRepository.InsertAsync(holidayEntity);
            //    if (request.IsForAllTeams)
            //    {
            //        var teamIds = await _unitOfWork.TeamRepository.GetListAsync(request.CompanyId);
            //        affectedRow = await _unitOfWork.HolidayRepository.InsertHolidayToTeams(holidayId, teamIds.Select(x => x.TeamId).ToList());
            //    }
            //    else
            //    {
            //        affectedRow = await _unitOfWork.HolidayRepository.InsertHolidayToTeams(holidayId, request.Teams);
            //    }
            //    _unitOfWork.Commit();
            //}

            ////TODO: Fire "holidayCreated" event
            return _mapper.Map<HolidayDto>(entity);
        }
    }
}
