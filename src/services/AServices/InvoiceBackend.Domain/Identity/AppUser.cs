using InvoiceBackend.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InvoiceBackend.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<int>, IEntity<int>, ICreatedByEntity, IModifiedByEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
    public int? ModifiedByUserId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public int? CreatedByUserId { get; set; }
}
