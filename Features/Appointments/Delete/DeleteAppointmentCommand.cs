using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Appointments.Delete
{
    public record DeleteAppointmentCommand(int Id)
        : IRequest<Result<bool>>;
}
