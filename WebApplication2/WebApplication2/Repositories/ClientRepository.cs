using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApbdContext _context;

    public ClientRepository(ApbdContext context)
    {
        _context = context;
    }

    public async Task<bool> CanDeleteClientAsync(int clientId)
    {
        return !await _context.ClientTrips.AnyAsync(ct => ct.IdClient == clientId);
    }

    public async Task DeleteClientAsync(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> ClientExistsByPeselAsync(string pesel)
    {
        return await _context.Clients.AnyAsync(c => c.Pesel == pesel);
    }
}