using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using FluentValidation;

namespace ClubeBeneficios.Customers.Api.Validators;

public sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}