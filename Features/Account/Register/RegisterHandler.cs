using MediatR;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Account.Register
{
    public class RegisterPatientHandler : IRequestHandler<RegisterPatientCommand, Result<string>>
    {
        private readonly TelemedicineDbContext _context;

        public RegisterPatientHandler(TelemedicineDbContext context) => _context = context;

        public async Task<Result<string>> Handle(
            RegisterPatientCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result<string>.Failure("Email already registered.");

            var patientExists = await _context.Patients
                .AnyAsync(p => p.Id == request.PatientId, cancellationToken);
            if (!patientExists)
                return Result<string>.Failure(
                    $"Patient with Id {request.PatientId} not found.");

            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Patient",
                RefId = request.PatientId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Ok(user.Email, "Registration successful.");
        }
    }

    public class RegisterDoctorHandler : IRequestHandler<RegisterDoctorCommand, Result<string>>
    {
        private readonly TelemedicineDbContext _context;

        public RegisterDoctorHandler(TelemedicineDbContext context) => _context = context;

        public async Task<Result<string>> Handle(
            RegisterDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result<string>.Failure("Email already registered.");

            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);
            if (!doctorExists)
                return Result<string>.Failure(
                    $"Doctor with Id {request.DoctorId} not found.");

            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Doctor",
                RefId = request.DoctorId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Ok(user.Email, "Registration successful.");
        }
    }
}
