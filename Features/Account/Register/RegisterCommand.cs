using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Account.Register
{
    public record RegisterPatientCommand(
        string Email,
        string Password,
        string FullName,
        string? Phone
    ) : IRequest<Result<string>>;

    public record RegisterDoctorCommand(
        string Email,
        string Password,
        string FullName,
        string? Specialty
    ) : IRequest<Result<string>>;
}
