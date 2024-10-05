namespace BankingAPI.DataAccess;

using BankingAPI.Models;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetClientsAsync(
        int pageIndex,
        int pageSize,
        string? filterBy = null,
        string? sortBy = null,
        string? searchValue = null
    );
    Task AddClientAsync(Client client);
    Task<List<string>> GetLastThreeSearchesAsync();
    Task SaveSearchParametersAsync(
        string filterBy,
        string searchValue,
        int pageIndex,
        int pageSize
    );
}
