namespace BankingAPI.DataAccess;

using BankingAPI.DataAccess;

public interface IUnitOfWork : IDisposable
{
    IClientRepository Clients { get; }
    Task<int> SaveChanges();
}
