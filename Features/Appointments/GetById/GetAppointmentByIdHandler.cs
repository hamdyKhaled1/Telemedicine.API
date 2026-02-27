using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Features.Appointments.GetAll;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.GetById
{
    public class GetAppointmentByIdHandler
     : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentResponse>>
    {
        private readonly TelemedicineDbContext _context;

        public GetAppointmentByIdHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AppointmentResponse>> Handle(
     GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment is null)
                return Result<AppointmentResponse>.Failure(
                    $"Appointment with Id {request.Id} not found.");

            return Result<AppointmentResponse>.Ok(new AppointmentResponse(
                appointment.Id,
                appointment.DoctorId,
                appointment.PatientId,
                appointment.StartTime,
                appointment.EndTime,
                appointment.Status));
        }
    }
}
