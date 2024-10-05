namespace BankingAPI.WebAPI.Controllers;

using BankingAPI.DataAccess;
using BankingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientController(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetClients(
        int pageIndex = 1,
        int pageSize = 10,
        string? filterBy = null,
        string? sortBy = null,
        string? searchValue = null
    )
    {
        // Retrieve a paginated list of clients with optional filtering and sorting...
        var clients = await _unitOfWork.Clients.GetClientsAsync(
            pageIndex,
            pageSize,
            filterBy,
            sortBy,
            searchValue
        );
        return Ok(clients);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddClient([FromBody] Client client)
    {
        // Add a new client to the database...
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _unitOfWork.Clients.AddClientAsync(client);
        return Ok("Client Added Successfully.");
    }

    [HttpGet("search-suggestions")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSearchSuggestions()
    {
        // Retrieve the last three search filter parameters as suggestions for the Admin...
        var suggestions = await _unitOfWork.Clients.GetLastThreeSearchesAsync();
        return Ok(suggestions);
    }

    [HttpGet("pagination-settings")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaginationSettings()
    {
        // Retrieve the last used pagination settings for Admin to view current pagination state...
        var paginationSettings = await _unitOfWork.Clients.GetLastPaginationParametersAsync();
        return Ok(
            new { PageIndex = paginationSettings.pageIndex, PageSize = paginationSettings.pageSize }
        );
    }

    [HttpPost("save-filter")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveSearchFilterParameters(string filterBy, string searchValue)
    {
        // Persist filter parameters separately in the cache for search suggestion tracking...
        await _unitOfWork.Clients.SaveSearchFilterParametersAsync(filterBy, searchValue);
        return Ok("Search filter parameters saved successfully.");
    }

    [HttpPost("save-filter-pagination")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveSearchAndPagination(
        string filterBy,
        string searchValue,
        int pageIndex,
        int pageSize
    )
    {
        // Save both filtering and pagination parameters combined for search and pagination tracking...
        await _unitOfWork.Clients.SaveSearchParametersAndPaginationAsync(
            filterBy,
            searchValue,
            pageIndex,
            pageSize
        );
        return Ok("Search filter and pagination parameters saved successfully.");
    }
}
