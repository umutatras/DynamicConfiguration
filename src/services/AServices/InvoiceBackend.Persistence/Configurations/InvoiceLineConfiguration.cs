using InvoiceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistence.Configurations
{
    public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceLine> builder)
        {
            builder.HasKey(il => il.Id);
            builder.Property(il => il.ItemName).IsRequired().HasMaxLength(200);
            builder.Property(il => il.Quentity).IsRequired();
            builder.Property(il => il.Price).HasColumnType("decimal(18,2)");
            builder.HasOne(s => s.Invoice).WithMany(s => s.InvoiceLines).HasForeignKey(il => il.InvoiceId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
