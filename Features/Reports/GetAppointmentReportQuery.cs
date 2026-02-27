using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Reports
{
    public record GetAppointmentReportQuery(int? Id)
        : IRequest<Result<byte[]>>;
}
