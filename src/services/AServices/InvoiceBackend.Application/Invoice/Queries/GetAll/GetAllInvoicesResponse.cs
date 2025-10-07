using InvoiceBackend.Application.Models.Invoice;

namespace InvoiceBackend.Application.Invoice.Queries.GetAll;

public class GetAllInvoicesResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceUpdateRequestDto> InvoiceLines { get; set; } = new List<InvoiceUpdateRequestDto>();
}
