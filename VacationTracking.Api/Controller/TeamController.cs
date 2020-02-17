using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ApiControllerBase
    {
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

    }
}