using FluentValidation;

namespace InvoiceBackend.Application.Invoice.Commands.Add;

public sealed class InvoiceAddCommandValidator : AbstractValidator<InvoiceAddCommand>
{
    public InvoiceAddCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is not null");
    }
}
