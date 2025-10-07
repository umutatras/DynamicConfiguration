namespace InvoiceBackend.Domain.Common;

public interface ICreatedByEntity
{
    DateTimeOffset CreatedOn { get; set; }
    int? CreatedByUserId { get; set; }
}
