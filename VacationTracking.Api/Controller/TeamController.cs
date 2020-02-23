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
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";
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
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetTeamQuery(id, companyId)));
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
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetTeamListQuery(companyId)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TeamDto>> CreateTeamAsync([FromBody]TeamModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new CreateTeamCommand(companyId, userId, model.Name, model.Members, model.Approvers);

            return Single(await QueryAsync(request));
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TeamDto>> UpdateTeamAsync(Guid id, [FromBody]TeamModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new UpdateTeamCommand(companyId, userId, id, model.Name, model.Members, model.Approvers);

            return Single(await QueryAsync(request));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteTeamAsync(Guid id)
        {
            Guid companyId = new Guid(_companyId);
            
            var request = new DeleteTeamCommand(id, companyId);

            return Single(await QueryAsync(request));
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