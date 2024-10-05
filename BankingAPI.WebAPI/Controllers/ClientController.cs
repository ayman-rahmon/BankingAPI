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
    public async Task<IActionResult> GetClients(int pageIndex = 1, int pageSize = 10)
    {
        var clients = await _unitOfWork.Clients.GetClientsAsync(pageIndex, pageSize);
        return Ok(clients);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> addClient([FromBody] Client client)
    {
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
        var suggestions = await _unitOfWork.Clients.GetLastThreeSearchesAsync();
        return Ok(suggestions);
    }
}
