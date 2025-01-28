using LazardCantine.Models;

namespace LazardCantine.Infrastructure;

public interface IClientRepository
{
    void Ajouter(Client client);
    Client? ObtenirParId(Guid id);
    void MettreAJour(Client client);
    List<Client> ObtenirTous();
}