using MediatR;

namespace Telemedicine.API.Features.Reports
{
    public record GetAppointmentReportQuery(int? Id) : IRequest<byte[]>;
}
