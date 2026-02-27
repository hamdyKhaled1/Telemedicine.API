using FluentValidation;
using MediatR;

namespace Telemedicine.API.Common
{
    public class ValidationBehavior<TRequest, TResponse>
          : IPipelineBehavior<TRequest, TResponse>
          where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (failures.Any())
            {
                // بدل throw new ValidationException(failures);
                var errors = failures.Select(e => e.ErrorMessage).ToList();
                // مش هينفع ترجع Result هنا مباشرة لأن TResponse مش دايما Result
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
