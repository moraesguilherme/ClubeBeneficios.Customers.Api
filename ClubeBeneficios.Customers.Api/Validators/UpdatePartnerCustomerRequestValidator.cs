using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using FluentValidation;

namespace ClubeBeneficios.Customers.Api.Validators;

public sealed class UpdatePartnerCustomerRequestValidator : AbstractValidator<UpdatePartnerCustomerRequest>
{
    public UpdatePartnerCustomerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);
    }
}