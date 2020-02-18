using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ApiControllerBase
    {
        // User should has admin or owner permission

        public TeamController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get team by id
        /// </summary>
        /// <param name="id">Id of team</param>
        /// <returns>Team information</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TeamDto>> GetTeamAsync(Guid id)
        {
            return Single(await QueryAsync(new GetTeamQuery(id)));
        }

        /// <summary>
        /// Get team list
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<TeamDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<TeamDto>>> GetAsync()
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid("3e4a39de-386a-4b1c-b42e-262549ed11e0");

            return Single(await QueryAsync(new GetTeamListQuery(companyId)));
        }

        /// <summary>
        /// Get team list
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [Route("test")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Test()
        {
            return Ok("perfect");
        }

    }
}