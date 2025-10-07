using FluentValidation;

namespace InvoiceBackend.Application.Customer.Commands.Delete;

public sealed class CustomerDeleteCommandValidator : AbstractValidator<CustomerDeleteCommand>
{
    public CustomerDeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is not null");

    }

}