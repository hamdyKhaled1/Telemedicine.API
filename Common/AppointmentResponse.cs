using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Common
{
    public record AppointmentResponse(
         int Id,
         int DoctorId,
         int PatientId,
         DateTime StartTime,
         DateTime EndTime,
         AppointmentStatus Status
     );
}
