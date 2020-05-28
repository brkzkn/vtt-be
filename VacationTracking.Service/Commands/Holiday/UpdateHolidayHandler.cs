using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.Holiday
{
    public class UpdateHolidayHandler : IRequestHandler<UpdateHolidayCommand, HolidayDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateHolidayHandler(IUnitOfWork unitOfWork, ILogger<UpdateHolidayHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<HolidayDto> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //Domain.Models.Holiday entity = await _unitOfWork.HolidayRepository.GetAsync(request.CompanyId, request.HolidayId);
            //if (entity == null)
            //    throw new ArgumentNullException(nameof(Domain.Models.Holiday));

            //entity.HolidayId = request.HolidayId;
            //entity.CompanyId = request.CompanyId;
            //entity.EndDate = request.EndDate;
            //entity.HolidayName = request.Name;
            //entity.IsFullDay = request.IsFullDay;
            //entity.StartDate = request.StartDate;
            //entity.UpdatedAt = DateTime.UtcNow;
            //entity.UpdatedBy = request.UserId;

            //using (_unitOfWork)
            //{
            //    _unitOfWork.Begin();
            //    // Delete teams of holiday
            //    var affectedRow = await _unitOfWork.HolidayRepository.RemoveTeamHolidays(request.HolidayId);

            //    affectedRow = await _unitOfWork.HolidayRepository.UpdateAsync(request.HolidayId, entity);
            //    if (request.IsForAllTeams)
            //    {
            //        var teamIds = await _unitOfWork.TeamRepository.GetListAsync(request.CompanyId);
            //        affectedRow = await _unitOfWork.HolidayRepository.InsertHolidayToTeams(request.HolidayId, teamIds.Select(x => x.TeamId).ToList());
            //    }
            //    else
            //    {
            //        affectedRow = await _unitOfWork.HolidayRepository.InsertHolidayToTeams(request.HolidayId, request.Teams);
            //    }

            //    _unitOfWork.Commit();
            //}

            ////TODO: Fire "holidayUpdated" event
            //return _mapper.Map<HolidayDto>(entity);
        }
    }
}
