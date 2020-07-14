using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";

        public CompanyController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get company by id
        /// </summary>
        /// <param name="id">Id of company</param>
        /// <returns>Company information</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CompanyDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Obsolete]
        public async Task<ActionResult<CompanyDto>> GetCompanyAsync(int id)
        {
            throw new NotImplementedException();
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            //return Single(await QueryAsync(new GetCompanyQuery(id, companyId)));
        }

        /// <summary>
        /// Set company settings
        /// </summary>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Obsolete]
        public async Task<ActionResult> UpdateInfo([FromBody]CompanySettingsModel model)
        {
            throw new NotImplementedException();
        }

    }
}