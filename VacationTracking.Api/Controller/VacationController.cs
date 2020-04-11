using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";

        public VacationController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost]
        [ProducesResponseType(typeof(VacationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VacationDto>> CreateVacationAsync([FromBody]VacationModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new CreateVacationCommand(companyId,
                                                    userId,
                                                    model.LeaveTypeId,
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.Reason);

            return Single(await CommandAsync(request));
        }


    }
}