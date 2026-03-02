using Telemedicine.API.Features.Account;

namespace Telemedicine.API.Common.JWTService
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
