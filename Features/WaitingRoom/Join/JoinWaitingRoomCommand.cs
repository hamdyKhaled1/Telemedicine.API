using MediatR;

namespace Telemedicine.API.Features.WaitingRoom.Join
{
    public record JoinWaitingRoomCommand(int AppointmentId) : IRequest;
}
