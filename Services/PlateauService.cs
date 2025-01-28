using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;

namespace LazardCantine.Services;

public class PlateauService(IClientRepository _clientRepository) : IPlateauService
{
    public (bool Succes, string Message, Ticket? Ticket) PayerPlateau(Guid clientId, PlateauDto plateauDto)
    {
        var client = _clientRepository.ObtenirParId(clientId);
        if (client == null)
            return (false, "Client introuvable", null);

        // Vérification de la complétude du plateau
        bool plateauComplet = ContientPlateauFixe(plateauDto.Produits);

        // Calcul du total selon la complétude
        decimal total = plateauComplet ? 10m : plateauDto.Produits.Sum(p => p.Price);
        var reduction = CalculeReduction(client, total);

        // Ajuster le montant final et s'assurer qu'il n'est pas négatif
        var montantFinal = total - reduction;
        montantFinal = Math.Max(montantFinal, 0);

        // Vérifier si le paiement est possible
        if (client.Balance < montantFinal && client.ClientType != ClientType.Interne && client.ClientType != ClientType.VIP)
        {
            return (false, "Solde insuffisant", null);
        }

        // Débiter le compte du client
        client.Balance -= montantFinal;
        _clientRepository.MettreAJour(client);

        // Générer un objet Ticket
        var ticket = new Ticket
        {
            ClientName = client.Name,
            ClientType = client.ClientType.ToString(),
            Produits = plateauDto.Produits,
            Total = total,
            Reduction = reduction,
            TotalFinal = montantFinal
        };

        // Retourner un message selon la complétude du plateau
        string message = plateauComplet
            ? "Paiement réussi avec le prix fixe de 10€."
            : "Paiement réussi, plateau incomplet, prix calculé à la pièce).";

        return (true, message, ticket);
    }

    private static decimal CalculeReduction(Client client, decimal total)
    {
        return client.ClientType switch
        {
            ClientType.Interne => 7.5m,
            ClientType.Prestataire => 6m,
            ClientType.VIP => total,
            ClientType.Stagiaire => 10m,
            ClientType.Visiteur => 0m,
            _ => 0m
        };
    }

    private static bool ContientPlateauFixe(List<Produit> produits)
    {
        return produits.Any(p => p.Type == ProductType.Entree) &&
               produits.Any(p => p.Type == ProductType.Plat) &&
               produits.Any(p => p.Type == ProductType.Dessert) &&
               produits.Any(p => p.Type == ProductType.Pain);
    }
}