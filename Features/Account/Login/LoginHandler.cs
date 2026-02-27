using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Common.JWTService;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Account.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly TelemedicineDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginHandler(TelemedicineDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isValid)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);

            return Result<LoginResponse>.Ok(
                new LoginResponse(token, user.Email, user.Role),
                "Login successful.");
        }
    }
}
