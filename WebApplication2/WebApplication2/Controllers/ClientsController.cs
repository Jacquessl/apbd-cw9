using Microsoft.AspNetCore.Mvc;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _clientRepository;

    public ClientsController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        if (!await _clientRepository.CanDeleteClientAsync(idClient))
        {
            return BadRequest("Cannot delete client with associated trips.");
        }

        await _clientRepository.DeleteClientAsync(idClient);
        return Ok();
    }
    
}