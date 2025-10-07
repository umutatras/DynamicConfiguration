using InvoiceBackend.Application.Models.General;
using InvoiceBackend.Application.Models.Invoice;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Commands.Add;

public sealed class InvoiceAddCommand : IRequest<ResponseDto<bool>>
{
    public int CustomerId { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceAddRequestDto> InvoiceLines { get; set; } = new List<InvoiceAddRequestDto>();
}
