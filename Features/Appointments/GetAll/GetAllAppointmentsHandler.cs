using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.GetAll
{
    public class GetAllAppointmentsHandler: IRequestHandler<GetAllAppointmentsQuery, Result<List<AppointmentResponse>>>
    {
        private readonly TelemedicineDbContext _context;

        public GetAllAppointmentsHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executes query and maps database entities to DTO list.
        /// </summary>
        public async Task<Result<List<AppointmentResponse>>> Handle(
            GetAllAppointmentsQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _context.Appointments
       .Select(a => new AppointmentResponse(
           a.Id,
           a.DoctorId,
           a.PatientId,
           a.StartTime,
           a.EndTime,
           a.Status))
       .ToListAsync(cancellationToken);

            return Result<List<AppointmentResponse>>.Ok(
                appointments,
                $"{appointments.Count} appointments found.");
           
        }
    }
}
