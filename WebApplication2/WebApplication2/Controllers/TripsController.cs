using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ApbdContext _context;
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientRepository _clientRepository;

    public TripsController(ApbdContext context, ITripsRepository tripsRepository, IClientRepository clientRepository)
    {
        _tripsRepository = tripsRepository;
        _context = context;
        _clientRepository = clientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips( int page = 1, int pageSize = 10)
    {
        var trips = await _tripsRepository.GetTrips(page, pageSize);
        var totalTrips = await _tripsRepository.GetTotalTripsCountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        return Ok(new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = totalPages,
            trips = trips
        });
    }
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> RegisterClientForTrip(int idTrip, string pesel)
    {
        if (await _clientRepository.ClientExistsByPeselAsync(pesel))
        {
            return BadRequest("Client with this PESEL already exists.");
        }

        if (await _tripsRepository.ClientIsRegisteredForTripAsync(idTrip, pesel))
        {
            return BadRequest("Client is already registered for this trip.");
        }

        if (!await _tripsRepository.TripExistsAndIsInFutureAsync(idTrip))
        {
            return BadRequest("Cannot register for a trip that has already taken place.");
        }

        await _tripsRepository.RegisterClientForTripAsync(pesel, idTrip);
        return CreatedAtAction(nameof(RegisterClientForTrip), new { idTrip = idTrip });
    }
}