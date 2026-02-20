using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Hubs;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.WaitingRoom.Join
{
    public class JoinWaitingRoomHandler: IRequestHandler<JoinWaitingRoomCommand>
    {
        private readonly TelemedicineDbContext _context;
        private readonly IHubContext<WaitingRoomHub> _hub;

        public JoinWaitingRoomHandler(
            TelemedicineDbContext context,
            IHubContext<WaitingRoomHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task Handle(
            JoinWaitingRoomCommand request,
            CancellationToken cancellationToken)
        {
            var room = await _context.WaitingRooms
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId);

            if (room == null)
                throw new Exception("Waiting room not found.");

            room.IsPatientJoined = true;
            room.JoinTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var appointment = await _context.Appointments
                .FindAsync(request.AppointmentId);

            await _hub.Clients.Group($"Doctor_{appointment.DoctorId}")
                .SendAsync("ReceiveNotification",
                    "Patient joined waiting room");

            
        }

        
    }
}
