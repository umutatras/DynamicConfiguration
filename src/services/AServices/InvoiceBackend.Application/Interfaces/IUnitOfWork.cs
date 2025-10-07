namespace InvoiceBackend.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
    }
}
