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

    public async Task<IEnumerable<Client>> GetClientsAsync(int pageIndex, int pageSize)
    {
        // getting the Clients in pages ... the skip will allow me to skip a number of rows calculated by the (pageIndex - 1 ) * pageSize , take will allow me to take only the amount of records i want (pageSize)...
        return await _context.Clients.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
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

    public async Task SaveSearchParametersAsync(string searchParams)
    {
        // updating the current cached Searches by adding a new one to the list while removing the oldest search term in case we hit the capacity (3 search terms)...
        var currentSearches = await GetLastThreeSearchesAsync();
        if (currentSearches.Count >= 3)
        {
            currentSearches.RemoveAt(0);
        }

        currentSearches.Add(searchParams);

        await _cache.SetStringAsync("searchParams", JsonConvert.SerializeObject(currentSearches));
    }
}
