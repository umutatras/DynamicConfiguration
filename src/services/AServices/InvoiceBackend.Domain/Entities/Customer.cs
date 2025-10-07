using InvoiceBackend.Domain.Common;

namespace InvoiceBackend.Domain.Entities;

public class Customer : EntityBase<int>
{
    public string TaxNumber { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string EMail { get; set; }
    public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();

}
