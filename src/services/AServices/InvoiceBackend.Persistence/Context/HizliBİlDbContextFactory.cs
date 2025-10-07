using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace InvoiceBackend.Persistence.Context
{
    public class HizliBİlDbContextFactory : IDesignTimeDbContextFactory<HizliBİlDbContext>
    {
        public HizliBİlDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();
            var optionsBuilder = new DbContextOptionsBuilder<HizliBİlDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new HizliBİlDbContext(optionsBuilder.Options);
        }
    }
}
