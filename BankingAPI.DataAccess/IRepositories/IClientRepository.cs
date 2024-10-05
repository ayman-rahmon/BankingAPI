namespace BankingAPI.DataAccess;

using BankingAPI.Models;

public interface IClientRepository
{
    // Retrieves a paginated list of clients with optional filtering and sorting parameters...
    Task<IEnumerable<Client>> GetClientsAsync(
        int pageIndex,
        int pageSize,
        string? filterBy = null,
        string? sortBy = null,
        string? searchValue = null
    );

    // Adds a new client record to the database...
    Task AddClientAsync(Client client);

    // Retrieves the last three search parameters for search suggestions...
    Task<List<string>> GetLastThreeSearchesAsync();

    // Retrieves the last used pagination settings from the cache...
    Task<(int pageIndex, int pageSize)> GetLastPaginationParametersAsync();

    // Saves filtering parameters in cache for the last three searches...
    Task SaveSearchFilterParametersAsync(string filterBy, string searchValue);

    // Saves both filtering and pagination parameters in cache for combined persistence...
    Task SaveSearchParametersAndPaginationAsync(
        string filterBy,
        string searchValue,
        int pageIndex,
        int pageSize
    );
}
