using FluentValidation;

namespace Telemedicine.API.Features.Appointments.Delete
{
    public class DeleteAppointmentValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Appointment Id must be a valid positive number.");
        }
    }
}
