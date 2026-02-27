using FluentValidation;

namespace Telemedicine.API.Features.Account.Register
{
    public class RegisterPatientValidator : AbstractValidator<RegisterPatientCommand>
    {
        public RegisterPatientValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be a valid positive number.");
        }
    }

    public class RegisterDoctorValidator : AbstractValidator<RegisterDoctorCommand>
    {
        public RegisterDoctorValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be a valid positive number.");
        }
    }
}
