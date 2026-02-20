using MediatR;
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

        /// <summary>
        /// Returns appointment report as PDF file.
        /// Example:
        /// GET /api/reports/appointments?id=1
        /// GET /api/reports/appointments
        /// </summary>
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointmentsReport([FromQuery] int? id)
        {
            var pdfBytes = await _mediator
                .Send(new GetAppointmentReportQuery(id));

            return File(
                pdfBytes,
                "application/pdf",
                "AppointmentsReport.pdf"
            );
        }
    }
}
