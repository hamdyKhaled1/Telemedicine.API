using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Account.Register
{
    public record RegisterPatientCommand(
        string Email,
        string Password,
        int PatientId
    ) : IRequest<Result<string>>;

    public record RegisterDoctorCommand(
        string Email,
        string Password,
        int DoctorId
    ) : IRequest<Result<string>>;
}
