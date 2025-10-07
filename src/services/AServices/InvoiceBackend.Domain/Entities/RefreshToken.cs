using InvoiceBackend.Domain.Common;

namespace InvoiceBackend.Domain.Entities;

public sealed class RefreshToken : EntityBase<int>
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string SecurityStamp { get; set; }

    public int AppUserId { get; set; }
}