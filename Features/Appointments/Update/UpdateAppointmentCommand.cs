using MediatR;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Update
{
    public record UpdateAppointmentCommand(
        int Id,
        int DoctorId,
        DateTime StartTime,
        DateTime EndTime,
        AppointmentStatus Status
    ) : IRequest<Result<AppointmentResponse>>;
}
