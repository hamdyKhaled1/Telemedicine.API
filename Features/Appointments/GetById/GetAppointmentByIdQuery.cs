using MediatR;
using Telemedicine.API.Features.Appointments.GetAll;

namespace Telemedicine.API.Features.Appointments.GetById
{
    public record GetAppointmentByIdQuery(int Id)
    : IRequest<AppointmentDto>;
}
