using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Create
{
    public class CreateAppointmentHandler
        : IRequestHandler<CreateAppointmentCommand, Result<AppointmentResponse>>
    {
        private readonly TelemedicineDbContext _context;

        public CreateAppointmentHandler(TelemedicineDbContext context)
            => _context = context;

        public async Task<Result<AppointmentResponse>> Handle(
            CreateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);
            if (!doctorExists)
                return Result<AppointmentResponse>.Failure(
                    $"Doctor with Id {request.DoctorId} not found.");

            var patientExists = await _context.Patients
                .AnyAsync(p => p.Id == request.PatientId, cancellationToken);
            if (!patientExists)
                return Result<AppointmentResponse>.Failure(
                    $"Patient with Id {request.PatientId} not found.");

            var overlap = await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == request.DoctorId &&
                    request.StartTime < a.EndTime &&
                    request.EndTime > a.StartTime,
                    cancellationToken);
            if (overlap)
                return Result<AppointmentResponse>.Failure(
                    "This time slot is already booked for the selected doctor.");

            var appointment = new Appointment
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = AppointmentStatus.Scheduled
            };

            _context.Appointments.Add(appointment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<AppointmentResponse>.Failure(
                    "A conflict occurred while booking. Please try again.");
            }

            return Result<AppointmentResponse>.Ok(
                new AppointmentResponse(
                    appointment.Id,
                    appointment.DoctorId,
                    appointment.PatientId,
                    appointment.StartTime,
                    appointment.EndTime,
                    appointment.Status),
                "Appointment created successfully.");
        }
    }
}
