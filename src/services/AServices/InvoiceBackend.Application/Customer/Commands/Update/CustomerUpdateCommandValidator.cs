using FluentValidation;

namespace InvoiceBackend.Application.Customer.Commands.Update;

public sealed class CustomerUpdateCommandValidator : AbstractValidator<CustomerUpdateCommand>
{
    public CustomerUpdateCommandValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage("TaxNumber is not null");

    }

}