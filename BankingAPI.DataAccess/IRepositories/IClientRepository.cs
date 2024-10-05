namespace BankingAPI.DataAccess;

using BankingAPI.Models;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetClientsAsync(int pageIndex, int pageSize);
    Task AddClientAsync(Client client);
    Task<List<string>> GetLastThreeSearchesAsync();
    Task SaveSearchParametersAsync(string searchParams);
}
