namespace InvoiceBackend.Application.Models.Invoice;

public class InvoiceUpdateRequestDto
{
    public string ItemName { get; set; }
    public int Quentity { get; set; }
    public decimal Price { get; set; }
}
