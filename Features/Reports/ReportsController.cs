using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Telemedicine.API.Features.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// Returns appointment report as PDF file.

        
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointmentsReport([FromQuery] int? id)
        {
            var result = await _mediator.Send(new GetAppointmentReportQuery(id));

            if (!result.Success)
                return NotFound(new { result.Success, result.Message });

            return File(result.Data!, "application/pdf", "AppointmentsReport.pdf");
        }
    
}
}
