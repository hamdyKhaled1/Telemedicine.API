using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Update
{
    public class UpdateAppointmentHandler
         : IRequestHandler<UpdateAppointmentCommand, Result<AppointmentResponse>>
    {
        private readonly TelemedicineDbContext _context;

        public UpdateAppointmentHandler(TelemedicineDbContext context)
            => _context = context;

        public async Task<Result<AppointmentResponse>> Handle(
            UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            // 1. الـ Appointment موجود؟
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment is null)
                return Result<AppointmentResponse>.Failure(
                    $"Appointment with Id {request.Id} not found.");

            // 2. الـ Doctor الجديد موجود؟
            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);

            if (!doctorExists)
                return Result<AppointmentResponse>.Failure(
                    $"Doctor with Id {request.DoctorId} not found.");

            // 3. في تعارض مع موعد تاني للدكتور؟
            var overlap = await _context.Appointments
                .AnyAsync(a =>
                    a.Id != request.Id &&
                    a.DoctorId == request.DoctorId &&
                    request.StartTime < a.EndTime &&
                    request.EndTime > a.StartTime,
                    cancellationToken);

            if (overlap)
                return Result<AppointmentResponse>.Failure(
                    "This time slot is already booked for the selected doctor.");

            // 4. عدل البيانات
            appointment.DoctorId = request.DoctorId;
            appointment.StartTime = request.StartTime;
            appointment.EndTime = request.EndTime;
            appointment.Status = request.Status;

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
                "Appointment updated successfully.");
        }
    }
}
