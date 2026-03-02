using MediatR;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Update.UpdateStatus
{
    public record UpdateAppointmentStatusCommand(
        int Id,
        AppointmentStatus NewStatus
    ) : IRequest<Result<AppointmentResponse>>;
}
