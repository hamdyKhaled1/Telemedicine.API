using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Features.Appointments.Create;
using Telemedicine.API.Features.Appointments.Delete;
using Telemedicine.API.Features.Appointments.GetAll;
using Telemedicine.API.Features.Appointments.GetById;

namespace Telemedicine.API.Features.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }


       
        /// Retrieves all appointments.
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAppointmentsQuery());
            return Ok(result);
        }

        
        /// Retrieves single appointment by id.
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetAppointmentByIdQuery(id));

            return Ok(result);
        }


        
        /// Soft deletes appointment.
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAppointmentCommand(id));
            return Ok("Appointment soft deleted.");
        }
    }
}
