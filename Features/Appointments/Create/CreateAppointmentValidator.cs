using FluentValidation;

namespace Telemedicine.API.Features.Appointments.Create
{
    public class CreateAppointmentValidator: AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime);
        }
    }
}
