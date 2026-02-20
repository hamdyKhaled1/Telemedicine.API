using MediatR;

namespace Telemedicine.API.Features.Appointments.Delete
{
    public record DeleteAppointmentCommand(int Id) : IRequest;
}
