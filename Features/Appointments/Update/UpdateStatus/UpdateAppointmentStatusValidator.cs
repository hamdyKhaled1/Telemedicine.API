using FluentValidation;

namespace Telemedicine.API.Features.Appointments.Update.UpdateStatus
{
    public class UpdateAppointmentStatusValidator
         : AbstractValidator<UpdateAppointmentStatusCommand>
    {
        public UpdateAppointmentStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid positive number.");

            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid status. Must be Scheduled, Completed, or Cancelled.");
        }
    }
}
