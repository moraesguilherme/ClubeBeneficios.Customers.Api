using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using FluentValidation;

namespace ClubeBeneficios.Customers.Api.Validators;

public sealed class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);
    }
}