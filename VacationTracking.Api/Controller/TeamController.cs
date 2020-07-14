using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const int _companyId = 1;
        private const int _userId = 1;
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
        public async Task<ActionResult<TeamDto>> GetTeamAsync(int id)
        {
            // TODO: Check permission. Only Admin and Owner account can run this
            // TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetTeamQuery(id, _companyId, _userId)));
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
            return Single(await QueryAsync(new GetTeamListQuery(_companyId)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TeamDto>> CreateTeamAsync([FromBody]TeamModel model)
        {
            var request = new CreateTeamCommand(_companyId, _userId, model.Name, model.Members, model.Approvers);

            return Single(await CommandAsync(request));
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TeamDto>> UpdateTeamAsync(int id, [FromBody]TeamModel model)
        {
            var request = new UpdateTeamCommand(_companyId, _userId, id, model.Name, model.Members, model.Approvers);

            return Single(await CommandAsync(request));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteTeamAsync(int id)
        {
            var request = new DeleteTeamCommand(id, _companyId);

            return Single(await CommandAsync(request));
        }
    }
}