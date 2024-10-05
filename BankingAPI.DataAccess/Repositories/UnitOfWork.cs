using BankingAPI.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _context;
    public IClientRepository Clients { get; }

    public UnitOfWork(ApplicationDBContext context, IClientRepository clients)
    {
        _context = context;
        Clients = clients;
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
