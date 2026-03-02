using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Features.Account.Login;
using Telemedicine.API.Features.Account.Register;

namespace Telemedicine.API.Features.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient(
            [FromBody] RegisterPatientCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register/doctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterDoctor(
            [FromBody] RegisterDoctorCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
    }
}

