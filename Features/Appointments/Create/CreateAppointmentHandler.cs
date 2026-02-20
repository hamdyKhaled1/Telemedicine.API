using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Create
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly TelemedicineDbContext _context;

        public CreateAppointmentHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        
        /// Creates appointment safely with overlap prevention.
       
        public async Task<int> Handle( CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database //هنا لي استخدمت الترانزاكشن عشان امنع التداخل بين المواعيد
                .BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            var overlap = await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == request.DoctorId &&// هنا بيقارن الدكتور اللي احنا محتاجينه
                    request.StartTime < a.EndTime && //(بداية الموعد الجديد قبل نهاية القديم)
                    request.EndTime > a.StartTime);//(نهاية الموعد الجديد بعد بداية القديم)

            if (overlap)
                throw new Exception("Time slot already booked.");

            var appointment = new Appointment
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = "Scheduled"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync();

            return appointment.Id;
        }
    }
}
