using InvoiceBackend.Domain.Common;

namespace InvoiceBackend.Domain.Entities;

public class InvoiceLine : EntityBase<int>
{
    public int InvoiceId { get; set; }
    public string ItemName { get; set; }
    public int Quentity { get; set; }
    public decimal Price { get; set; }

    #region Navigation Properties   
    public virtual Invoice Invoice { get; set; }
    #endregion
}
