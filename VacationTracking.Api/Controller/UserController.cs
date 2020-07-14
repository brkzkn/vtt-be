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
        private const int _companyId = 1;
        private const int _userId = 1;
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
        public async Task<ActionResult<UserDto>> GetTeamAsync(int id)
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetUserQuery(id, _companyId)));
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
            return Single(await QueryAsync(new GetUserQuery(_userId, _companyId)));
        }


        /// <summary>
        /// Get user list
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync()
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetUserListQuery(_companyId)));
        }
    }
}