namespace InvoiceBackend.Domain.Common;

public interface IModifiedByEntity
{
    DateTimeOffset? ModifiedOn { get; set; }
    int? ModifiedByUserId { get; set; }
}
