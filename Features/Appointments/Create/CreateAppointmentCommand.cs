using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Appointments.Create
{
    public record CreateAppointmentCommand(
     int DoctorId,
     int PatientId,
     DateTime StartTime,
     DateTime EndTime
 ) : IRequest<Result<AppointmentResponse>>;
}
