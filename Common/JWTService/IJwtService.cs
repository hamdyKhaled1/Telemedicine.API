namespace Telemedicine.API.Common.JWTService
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string role);
    }
}
