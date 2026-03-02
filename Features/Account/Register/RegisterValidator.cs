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

                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("FullName is required.")
                    .MaximumLength(150).WithMessage("FullName must not exceed 150 characters.");
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

                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("FullName is required.")
                    .MaximumLength(150).WithMessage("FullName must not exceed 150 characters.");
            }
        }
    }

