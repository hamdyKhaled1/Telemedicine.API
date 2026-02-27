using FluentValidation;

namespace Telemedicine.API.Features.Appointments.Update
{
    public class UpdateAppointmentValidator
       : AbstractValidator<UpdateAppointmentCommand>
    {
       
            public UpdateAppointmentValidator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Appointment Id must be a valid positive number.");

                RuleFor(x => x.DoctorId)
                    .GreaterThan(0).WithMessage("DoctorId must be a valid positive number.");

                RuleFor(x => x.StartTime)
                    .LessThan(x => x.EndTime).WithMessage("StartTime must be before EndTime.");

                RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.");

                RuleFor(x => x.Status)
                    .IsInEnum().WithMessage("Invalid status. Must be Scheduled, Completed, or Cancelled.");
            }
        }
    }

