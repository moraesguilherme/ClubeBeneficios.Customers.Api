using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using FluentValidation;

namespace ClubeBeneficios.Customers.Api.Validators;

public sealed class CreatePartnerCustomerRequestValidator : AbstractValidator<CreatePartnerCustomerRequest>
{
    public CreatePartnerCustomerRequestValidator()
    {
        RuleFor(x => x.PartnerId)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);
    }
}