using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Common;
using Telemedicine.API.Features.Appointments.Create;
using Telemedicine.API.Features.Appointments.Delete;
using Telemedicine.API.Features.Appointments.GetAll;
using Telemedicine.API.Features.Appointments.GetById;
using Telemedicine.API.Features.Appointments.Update;
using Telemedicine.API.Features.Appointments.Update.UpdateStatus;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Create(CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }



        /// Retrieves all appointments.

        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAppointmentsQuery());
            return Ok(result);
        }


        /// Retrieves  appointment by id.

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,Patient,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAppointmentByIdQuery(id));
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateAppointmentCommand command)
        {
            
            if (id != command.Id)
                return BadRequest(new { Success = false, Message = "Id in URL must match Id in body." });

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] AppointmentStatus newStatus)
        {
            var result = await _mediator.Send(
                new UpdateAppointmentStatusCommand(id, newStatus));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// delete appointment.

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAppointmentCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
