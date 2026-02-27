using MediatR;
using Telemedicine.API.Common;

namespace Telemedicine.API.Features.Account.Login
{
    public record LoginCommand(
         string Email,
         string Password
     ) : IRequest<Result<LoginResponse>>;
}
