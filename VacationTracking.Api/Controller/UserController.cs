using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.User;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <returns>User information</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetTeamAsync(Guid id)
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetUserQuery(id, companyId)));
        }

        /// <summary>
        /// Get logged-in user info
        /// </summary>
        /// <returns>Logged-in user information</returns>
        [HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetTeamAsync()
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            return Single(await QueryAsync(new GetUserQuery(userId, companyId)));
        }


        /// <summary>
        /// Get user list
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<UserDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<UserDto>>> GetAsync()
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetUserListQuery(companyId)));
        }
    }
}