using LazardCantine.Models;

namespace LazardCantine.Infrastructure;

public class ClientRepository : IClientRepository
{
    private readonly List<Client> _clients = new();

    public virtual void Ajouter(Client client) => _clients.Add(client);

    public Client? ObtenirParId(Guid id) => _clients.FirstOrDefault(c => c.Id == id);

    public void MettreAJour(Client client)
    {
        var index                       = _clients.FindIndex(c => c.Id == client.Id);
        if (index >= 0) _clients[index] = client;
    }

    public List<Client> ObtenirTous()
    {
        return _clients.ToList();
    }
}