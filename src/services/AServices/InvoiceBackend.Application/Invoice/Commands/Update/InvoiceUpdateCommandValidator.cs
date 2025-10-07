using FluentValidation;

namespace InvoiceBackend.Application.Invoice.Commands.Update;

public class InvoiceUpdateCommandValidator : AbstractValidator<InvoiceUpdateCommand>
{
    public InvoiceUpdateCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is not null");
    }
}
