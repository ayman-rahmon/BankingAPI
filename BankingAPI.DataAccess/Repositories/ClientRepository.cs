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

        // Filtering Logic...
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

            // Save filtering parameters for search suggestions separately...
            await SaveSearchFilterParametersAsync(filterBy, searchValue);
        }

        // Sorting Logic...
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "firstname" => query.OrderBy(c => c.FirstName),
                "lastname" => query.OrderBy(c => c.LastName),
                "email" => query.OrderBy(c => c.Email),
                _ => query // Default case, no sorting...
            };
        }

        // Save pagination parameters separately...
        await SavePaginationParametersAsync(pageIndex, pageSize);

        // Apply pagination and return results...
        return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task AddClientAsync(Client client)
    {
        // Adding the client entity to the context and saving it to the database...
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetLastThreeSearchesAsync()
    {
        // Fetching the last three search parameters from Redis cache for suggestions...
        var cachedSearches = await _cache.GetStringAsync("searchParams");
        return cachedSearches != null
            ? JsonConvert.DeserializeObject<List<string>>(cachedSearches).Take(3).ToList()
            : new List<string>();
    }

    public async Task<(int pageIndex, int pageSize)> GetLastPaginationParametersAsync()
    {
<<<<<<< HEAD
=======
        // Fetching the last used pagination parameters from Redis cache...
>>>>>>> development
        var cachedPagination = await _cache.GetStringAsync("paginationParams");
        if (cachedPagination != null)
        {
            var paginationParams = JsonConvert.DeserializeObject<Dictionary<string, int>>(
                cachedPagination
            );
            return (paginationParams["pageIndex"], paginationParams["pageSize"]);
        }
<<<<<<< HEAD
        return (1, 10); // Default pagination parameters if not set
    }

    private async Task SaveSearchFilterParametersAsync(string filterBy, string searchValue)
    {
=======
        return (1, 10); // Default pagination parameters if none are set in cache...
    }

    public async Task SaveSearchFilterParametersAsync(string filterBy, string searchValue)
    {
        // Construct the filtering parameters and store the last three entries in cache...
>>>>>>> development
        var currentSearches = await GetLastThreeSearchesAsync();
        var searchParams = $"filterBy:{filterBy}|searchValue:{searchValue}";

        if (currentSearches.Count >= 3)
        {
<<<<<<< HEAD
            currentSearches.RemoveAt(0);
        }

        currentSearches.Add(searchParams);
=======
            currentSearches.RemoveAt(0); // Remove the oldest search if limit is reached...
        }

        currentSearches.Add(searchParams);

>>>>>>> development
        await _cache.SetStringAsync("searchParams", JsonConvert.SerializeObject(currentSearches));
    }

    private async Task SavePaginationParametersAsync(int pageIndex, int pageSize)
    {
<<<<<<< HEAD
=======
        // Saving pagination settings to the cache...
>>>>>>> development
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

<<<<<<< HEAD
    public async Task SaveSearchParametersAsync(
=======
    public async Task SaveSearchParametersAndPaginationAsync(
>>>>>>> development
        string filterBy,
        string searchValue,
        int pageIndex,
        int pageSize
    )
    {
        // Storing combined filtering and pagination parameters in cache as a single entry...
        var currentSearches = await GetLastThreeSearchesAsync();
        var searchParams =
            $"filterBy:{filterBy}|searchValue:{searchValue}|pageIndex:{pageIndex}|pageSize:{pageSize}";

        if (currentSearches.Count >= 3)
        {
            currentSearches.RemoveAt(0); // Maintain only the last three entries...
        }

        currentSearches.Add(searchParams);

        await _cache.SetStringAsync("searchParams", JsonConvert.SerializeObject(currentSearches));
    }
}
