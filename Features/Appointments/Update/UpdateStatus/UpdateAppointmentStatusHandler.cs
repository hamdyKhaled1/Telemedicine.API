using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Update.UpdateStatus
{
    public class UpdateAppointmentStatusHandler
         : IRequestHandler<UpdateAppointmentStatusCommand, Result<AppointmentResponse>>
    {
        private readonly TelemedicineDbContext _context;

        public UpdateAppointmentStatusHandler(TelemedicineDbContext context)
            => _context = context;

        public async Task<Result<AppointmentResponse>> Handle(
            UpdateAppointmentStatusCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment is null)
                return Result<AppointmentResponse>.Failure(
                    $"Appointment with Id {request.Id} not found.");

            appointment.Status = request.NewStatus;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<AppointmentResponse>.Failure(
                    "A conflict occurred while updating. Please try again.");
            }

            return Result<AppointmentResponse>.Ok(
                new AppointmentResponse(
                    appointment.Id,
                    appointment.DoctorId,
                    appointment.PatientId,
                    appointment.StartTime,
                    appointment.EndTime,
                    appointment.Status),
                "Appointment status updated successfully.");
        }
    }
}
