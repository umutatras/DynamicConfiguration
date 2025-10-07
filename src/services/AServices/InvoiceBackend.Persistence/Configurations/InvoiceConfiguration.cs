using InvoiceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(50);
        builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
        builder.HasOne(i => i.Customer).WithMany(s => s.Invoices).HasForeignKey(i => i.CustomerId).OnDelete(DeleteBehavior.NoAction);
    }
}
