namespace BankingAPI.DataAccess;

using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IDistributedCache _cache;

    public ClientRepository(ApplicationDBContext context, IDistributedCache cache)
    {
        this._context = context;
        this._cache = cache;
    }

    public async Task<IEnumerable<Client>> GetClientsAsync(
        int pageIndex,
        int pageSize,
        string? filterBy = null,
        string? sortBy = null,
        string? searchValue = null
    )
    {
        IQueryable<Client> query = _context.Clients;

        // Filtering Logic
        if (!string.IsNullOrEmpty(filterBy) && !string.IsNullOrEmpty(searchValue))
        {
            switch (filterBy.ToLower())
            {
                case "firstname":
                    query = query.Where(c => c.FirstName.Contains(searchValue));
                    break;
                case "lastname":
                    query = query.Where(c => c.LastName.Contains(searchValue));
                    break;
                case "email":
                    query = query.Where(c => c.Email.Contains(searchValue));
                    break;
            }

            // Persist the filtering and pagination parameters for suggestions
            await SaveSearchParametersAsync(filterBy, searchValue, pageIndex, pageSize);
        }

        // Sorting Logic
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "firstname" => query.OrderBy(c => c.FirstName),
                "lastname" => query.OrderBy(c => c.LastName),
                "email" => query.OrderBy(c => c.Email),
                _ => query // Default case, no sorting
            };
        }

        // Pagination Logic
        return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task AddClientAsync(Client client)
    {
        // just adding the record as it is ... and updating the flushing the changes to DB...
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetLastThreeSearchesAsync()
    {
        // checking the already cached last 3 searches from the distributed cache syste (here using Redis)...
        var cachedSearches = await _cache.GetStringAsync("searchParams");
        return cachedSearches != null
            ? JsonConvert.DeserializeObject<List<string>>(cachedSearches).Take(3).ToList()
            : new List<string>();
    }

    public async Task<(int pageIndex, int pageSize)> GetLastPaginationParametersAsync()
    {
        var cachedPagination = await _cache.GetStringAsync("paginationParams");
        if (cachedPagination != null)
        {
            var paginationParams = JsonConvert.DeserializeObject<Dictionary<string, int>>(
                cachedPagination
            );
            return (paginationParams["pageIndex"], paginationParams["pageSize"]);
        }
        return (1, 10); // Default pagination parameters if not set
    }

    private async Task SaveSearchFilterParametersAsync(string filterBy, string searchValue)
    {
        var currentSearches = await GetLastThreeSearchesAsync();
        var searchParams = $"filterBy:{filterBy}|searchValue:{searchValue}";

        if (currentSearches.Count >= 3)
        {
            currentSearches.RemoveAt(0);
        }

        currentSearches.Add(searchParams);
        await _cache.SetStringAsync("searchParams", JsonConvert.SerializeObject(currentSearches));
    }

    private async Task SavePaginationParametersAsync(int pageIndex, int pageSize)
    {
        var paginationParams = new Dictionary<string, int>
        {
            { "pageIndex", pageIndex },
            { "pageSize", pageSize }
        };
        await _cache.SetStringAsync(
            "paginationParams",
            JsonConvert.SerializeObject(paginationParams)
        );
    }

    public async Task SaveSearchParametersAsync(
        string filterBy,
        string searchValue,
        int pageIndex,
        int pageSize
    )
    {
        var currentSearches = await GetLastThreeSearchesAsync();

        // Construct a full search string that includes all parameters...
        var searchParams =
            $"filterBy:{filterBy}|searchValue:{searchValue}|pageIndex:{pageIndex}|pageSize:{pageSize}";

        // Maintain only the last 3 search parameters...
        if (currentSearches.Count >= 3)
        {
            currentSearches.RemoveAt(0);
        }

        currentSearches.Add(searchParams);

        await _cache.SetStringAsync("searchParams", JsonConvert.SerializeObject(currentSearches));
    }
}
