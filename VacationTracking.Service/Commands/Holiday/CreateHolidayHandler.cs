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
    public class CreateHolidayHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateHolidayHandler(IUnitOfWork unitOfWork, ILogger<CreateHolidayHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //var holidayEntity = new Domain.Models.Holiday();
            //Guid holidayId = Guid.NewGuid();

            //holidayEntity.CompanyId = request.CompanyId;
            //holidayEntity.CreatedAt = DateTime.UtcNow;
            //holidayEntity.CreatedBy = request.UserId;
            //holidayEntity.EndDate = request.EndDate;
            //holidayEntity.HolidayId = holidayId;
            //holidayEntity.HolidayName = request.Name;
            //holidayEntity.IsFullDay = request.IsFullDay;
            //holidayEntity.StartDate = request.StartDate;

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
            //return _mapper.Map<HolidayDto>(holidayEntity);
        }
    }
}
