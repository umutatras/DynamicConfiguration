using FluentValidation;

namespace InvoiceBackend.Application.Invoice.Commands.Delete;

public class InvoiceDeleteCommandValidator : AbstractValidator<InvoiceDeleteCommand>
{
    public InvoiceDeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is not null");
    }
}
