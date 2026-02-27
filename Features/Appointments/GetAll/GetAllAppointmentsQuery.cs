using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Appointments.GetAll
{
    public record GetAllAppointmentsQuery()
    : IRequest<Result<List<AppointmentResponse>>>;
}
