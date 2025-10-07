using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Commands.Delete;

public sealed class CustomerDeleteCommand : IRequest<ResponseDto<bool>>
{
    public int Id { get; set; }

}
