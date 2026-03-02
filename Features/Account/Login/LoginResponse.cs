namespace Telemedicine.API.Features.Account.Login
{
    public record LoginResponse(
       string Token,
       string Email,
       string Role
   );
}
