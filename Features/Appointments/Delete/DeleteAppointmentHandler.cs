using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Appointments.Delete
{
    public class DeleteAppointmentHandler
         : IRequestHandler<DeleteAppointmentCommand, Result<bool>>
    {
        private readonly TelemedicineDbContext _context;

        public DeleteAppointmentHandler(TelemedicineDbContext context)
            => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment is null)
                return Result<bool>.Failure(
                    $"Appointment with Id {request.Id} not found.");

            // Soft Delete
            appointment.IsDeleted = true;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<bool>.Failure(
                    "A conflict occurred while deleting. Please try again.");
            }

            return Result<bool>.Ok(true, "Appointment deleted successfully.");
        }
    }
}
