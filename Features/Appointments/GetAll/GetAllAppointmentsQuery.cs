using MediatR;

namespace Telemedicine.API.Features.Appointments.GetAll
{
    public record GetAllAppointmentsQuery()
    : IRequest<List<AppointmentDto>>;
}
