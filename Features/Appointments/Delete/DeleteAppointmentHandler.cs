using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Delete
{
    public class DeleteAppointmentHandler
      : IRequestHandler<DeleteAppointmentCommand>
    {
        private readonly TelemedicineDbContext _context;

        public DeleteAppointmentHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Marks appointment as deleted without removing from database.
        /// </summary>
        public async Task Handle(
            DeleteAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.Id);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (appointment.IsDeleted)
                throw new Exception("Appointment already deleted.");

            appointment.IsDeleted = true;
            appointment.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            
        }
    }
}
