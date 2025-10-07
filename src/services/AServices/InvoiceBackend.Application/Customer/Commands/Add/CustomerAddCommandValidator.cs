using FluentValidation;

namespace InvoiceBackend.Application.Customer.Commands.Add;

public sealed class CustomerAddCommandValidator : AbstractValidator<CustomerAddCommand>
{
    public CustomerAddCommandValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage("TaxNumber is not null");

    }

}