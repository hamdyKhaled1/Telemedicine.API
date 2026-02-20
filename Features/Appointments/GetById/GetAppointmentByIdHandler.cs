using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Features.Appointments.GetAll;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.GetById
{
    public class GetAppointmentByIdHandler
     : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        private readonly TelemedicineDbContext _context;

        public GetAppointmentByIdHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto> Handle(
            GetAppointmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Id == request.Id && !a.IsDeleted)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    DoctorName = a.Doctor.FullName,
                    PatientName = a.Patient.FullName,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            return appointment;
        }
    }
}
