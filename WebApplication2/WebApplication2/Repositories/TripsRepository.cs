using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Models;

namespace WebApplication2.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly IConfiguration _configuration;
    private ITripsRepository _tripsRepositoryImplementation;
    private readonly ApbdContext _context;

    public TripsRepository(IConfiguration configuration, ApbdContext apbdContext)
    {
        _configuration = configuration;
        _context = apbdContext;
    }
    public async Task<IEnumerable<TripDTO>> GetTrips(int page, int pageSize)
    {
        return await _context.Trips.Select(e => new TripDTO()
            {
                Name = e.Name,
                DateFrom = e.DateFrom,
                MaxPeople = e.MaxPeople,
                Clients = e.ClientTrips.Select(e => new ClientDTO()
                {
                    FirstName = e.IdClientNavigation.FirstName,
                    LastName = e.IdClientNavigation.LastName
                })
            })
            .OrderBy(e => e.DateFrom)
            .ToListAsync();
    }

    public async Task<int> GetTotalTripsCountAsync()
    {
        return await _context.Trips.CountAsync();
    }
    public async Task<bool> ClientIsRegisteredForTripAsync(int tripId, string pesel)
    {
        var id = _context.Clients.Select(e => new Client()).Where(e => e.Pesel.Equals(pesel)).First();
        return await _context.ClientTrips.AnyAsync(ct => ct.IdTrip == tripId && ct.IdClient == id.IdClient);
    }

    public async Task<bool> TripExistsAndIsInFutureAsync(int tripId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        return trip != null && trip.DateFrom > DateTime.Now;
    }

    public async Task RegisterClientForTripAsync(string pesel, int tripId)
    {
        var client = _context.Clients.Where(e =>
            e.Pesel.Equals(pesel)
        ).First();
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = tripId,
            RegisteredAt = DateTime.Now,
            PaymentDate = null
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
}