using FluentValidation;

namespace Telemedicine.API.Features.Appointments.Create
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be a valid positive number.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be a valid positive number.");

            RuleFor(x => x.StartTime)
                .GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be in the future.");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime).WithMessage("StartTime must be before EndTime.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.");
        }
    }
}
