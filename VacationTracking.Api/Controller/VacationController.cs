using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Vacation;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const int _companyId = 1;
        private const int _userId = 1;

        public VacationController(IMediator mediator) : base(mediator)
        {

        }


        /// <summary>
        /// Get vacation list
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<VacationDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<VacationDto>>> GetAsync()
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetVacationListQuery(_companyId)));
        }

        /// <summary>
        /// Create vacation for logged-in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(VacationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VacationDto>> CreateVacationAsync([FromBody]VacationModel model)
        {
            //Guid companyId = new Guid(_companyId);
            //Guid userId = new Guid(_userId);
            int companyId = 1;
            int userId = 1;

            var request = new CreateVacationCommand(companyId,
                                                    userId,
                                                    12,//model.LeaveTypeId,
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.Reason,
                                                    model.IsHalfDays);

            return Single(await CommandAsync(request));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="model"></param>
        /// <returns>Created Vacation object</returns>
        [HttpPost]
        [Route("{id}/user")]
        [ProducesResponseType(typeof(VacationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VacationDto>> CreateUserVacationAsync(int id, [FromBody]VacationModel model)
        {
            var request = new CreateUserVacationCommand(_companyId,
                                                    id,
                                                    model.LeaveTypeId,
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.Reason);

            return Single(await CommandAsync(request));
        }

        [HttpPut]
        [Route("{id}/response")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> ResponseUserVacationAsync(int id, [FromBody]VacationResponseModel model)
        {
            var request = new UpdateVacationCommand(_companyId,
                                                    id,
                                                    _userId,
                                                    model.Status,
                                                    model.Note);

            return Single(await CommandAsync(request));
        }
    }
}