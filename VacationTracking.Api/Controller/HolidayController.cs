using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Holiday;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const int _companyId = 1;
        private const int _userId = 1;

        public HolidayController(IMediator mediator) : base(mediator)
        {

        }

        /// <summary>
        /// Get holiday by id
        /// </summary>
        /// <param name="id">Id of holiday</param>
        /// <returns>Holiday information</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HolidayDto>> GetHolidayAsync(int id)
        {
            //TODO: Set companyId from logged-in users

            return Single(await QueryAsync(new GetHolidayQuery(id, _companyId)));
        }

        /// <summary>
        /// Get holiday list
        /// </summary>
        /// <returns>List of holiday information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<HolidayDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<HolidayDto>>> GetHolidayListAsync()
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetHolidayListQuery(_companyId)));
        }

        /// <summary>
        /// Create holiday
        /// </summary>
        /// <returns>Return created HolidayDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<HolidayDto>> CreateHolidayAsync([FromBody]HolidayModel model)
        {
            var request = new CreateHolidayCommand(companyId: _companyId, 
                                                   userId: _userId, 
                                                   model.Teams, 
                                                   model.EndDate, 
                                                   model.StartDate, 
                                                   model.Name, 
                                                   model.IsForAllTeams,
                                                   model.IsFullDay
                                                   );

            return Single(await CommandAsync(request));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteHolidayAsync(int id)
        {
            var request = new DeleteHolidayCommand(id, _companyId);

            return Single(await CommandAsync(request));
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<HolidayDto>> UpdateTeamAsync(int id, [FromBody]HolidayModel model)
        {
            var request = new UpdateHolidayCommand(companyId : _companyId,
                                                   holidayId: id,
                                                   userId: _userId,
                                                   model.Name,
                                                   model.StartDate,
                                                   model.EndDate,                                                   
                                                   model.Teams,
                                                   model.IsFullDay,
                                                   model.IsForAllTeams);

            return Single(await CommandAsync(request));
        }
    }
}