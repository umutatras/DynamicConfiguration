using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Commands.Delete;

public sealed class InvoiceDeleteCommand : IRequest<ResponseDto<bool>>
{
    public int Id { get; set; }
}
