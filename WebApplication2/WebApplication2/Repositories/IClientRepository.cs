namespace WebApplication2.Repositories;

public interface IClientRepository
{
    Task<bool> CanDeleteClientAsync(int clientId);
    Task DeleteClientAsync(int clientId);
    Task<bool> ClientExistsByPeselAsync(string pesel);
}