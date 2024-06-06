using WebApplication2.DTOs;
using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface ITripsRepository
{
    Task<IEnumerable<TripDTO>> GetTrips(int page, int pageSize);
    Task<int> GetTotalTripsCountAsync();
    Task<bool> ClientIsRegisteredForTripAsync(int tripId, string pesel);
    Task<bool> TripExistsAndIsInFutureAsync(int tripId);
    Task RegisterClientForTripAsync(string pesel, int tripId);
}