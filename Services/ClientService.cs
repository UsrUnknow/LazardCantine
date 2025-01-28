using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;

namespace LazardCantine.Services;

public class ClientService(IClientRepository clientRepository)
{
    public Client CreerClient(ClientDto dto)
    {
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = dto.Name,
            ClientType = dto.Type,
            Balance    = dto.Balance
        };

        clientRepository.Ajouter(client);
        return client;
    }

    public Client? ObtenirClientParId(Guid id) => clientRepository.ObtenirParId(id);

    public bool CrediterCompte(Guid id, decimal montant)
    {
        var client = clientRepository.ObtenirParId(id);
        if (client == null) return false;

        client.Balance += montant;
        clientRepository.MettreAJour(client);
        return true;
    }
    
    public List<Client> ObtenirTousLesClients()
    {
        return clientRepository.ObtenirTous()
                               .Select(c => new Client
                               {
                                   Id         = c.Id,
                                   Name       = c.Name,
                                   ClientType = c.ClientType,
                                   Balance    = c.Balance
                               })
                               .ToList();
    }
}