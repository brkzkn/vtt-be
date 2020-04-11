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
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";

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
        public async Task<ActionResult<HolidayDto>> GetHolidayAsync(Guid id)
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetHolidayQuery(id, companyId)));
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
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetHolidayListQuery(companyId)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<HolidayDto>> CreateHolidayAsync([FromBody]HolidayModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new CreateHolidayCommand(companyId, 
                                                   userId, 
                                                   model.Teams, 
                                                   model.EndDate, 
                                                   model.StartDate, 
                                                   model.Name, 
                                                   model.IsForAllTeams, 
                                                   model.IsFullDay);

            return Single(await CommandAsync(request));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteHolidayAsync(Guid id)
        {
            Guid companyId = new Guid(_companyId);

            var request = new DeleteHolidayCommand(id, companyId);

            return Single(await CommandAsync(request));
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<HolidayDto>> UpdateTeamAsync(Guid id, [FromBody]HolidayModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new UpdateHolidayCommand(companyId,
                                                   id,
                                                   userId,
                                                   model.Name,
                                                   model.StartDate,
                                                   model.EndDate,
                                                   model.IsFullDay,
                                                   model.IsForAllTeams,
                                                   model.Teams);

            return Single(await CommandAsync(request));
        }


    }
}