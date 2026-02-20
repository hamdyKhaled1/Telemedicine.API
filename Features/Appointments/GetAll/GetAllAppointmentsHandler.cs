using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.GetAll
{
    public class GetAllAppointmentsHandler: IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly TelemedicineDbContext _context;

        public GetAllAppointmentsHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executes query and maps database entities to DTO list.
        /// </summary>
        public async Task<List<AppointmentDto>> Handle(
            GetAllAppointmentsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Appointments
                 .Where(a => !a.IsDeleted)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    DoctorName = a.Doctor.FullName,
                    PatientName = a.Patient.FullName,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status
                })
                .ToListAsync(cancellationToken);
        }
    }
}
