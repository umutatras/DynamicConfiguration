using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Commands.Add;

public sealed class CustomerAddCommand : IRequest<ResponseDto<bool>>
{
    public string TaxNumber { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string EMail { get; set; }
}
