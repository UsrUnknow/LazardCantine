using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;

namespace LazardCantine.Services;

public class ProduitService
{
    private readonly IProduitRepository _produitRepository;

    public ProduitService(IProduitRepository produitRepository)
    {
        _produitRepository = produitRepository;
    }

    /// <summary>
    /// Récupère tous les produits sous forme de DTOs.
    /// </summary>
    /// <returns>Liste des produits sous forme de DTO.</returns>
    public IEnumerable<ProduitDto> GetTousLesProduits()
    {
        var produits = _produitRepository.ObtenirTous();
        return produits.Select(p => new ProduitDto
        {
            Name = p.Name,
            Price = p.Price,
            Type = p.Type.ToString()
        });
    }

    /// <summary>
    /// Récupère un produit spécifique par son nom.
    /// </summary>
    /// <param name="nom">Nom du produit.</param>
    /// <returns>Un produit sous forme de DTO, ou null si le produit n'existe pas.</returns>
    public ProduitDto? GetProduitParNom(string nom)
    {
        var produit = _produitRepository.ObtenirParNom(nom);
        if (produit == null) return null;

        return new ProduitDto
        {
            Name = produit.Name,
            Price = produit.Price,
            Type = produit.Type.ToString()
        };
    }

    /// <summary>
    /// Ajoute un nouveau produit dans la base de données.
    /// </summary>
    /// <param name="produit">Produit à ajouter.</param>
    public void AjouterProduit(ProduitDto dto)
    {
        if (!Enum.TryParse<ProductType>(dto.Type, true, out var productType))
        {
            throw new ArgumentException($"Type de produit invalide : {dto.Type}");
        }

        var produit = new Produit()
        {
            Name = dto.Name,
            Price = dto.Price,
            Type = productType
        };
        
        _produitRepository.Ajouter(produit);
    }

    /// <summary>
    /// Supprime un produit par son nom.
    /// </summary>
    /// <param name="nom">Nom du produit à supprimer.</param>
    public bool SupprimerProduit(string nom)
    {
        var produitExist = _produitRepository.ObtenirParNom(nom);
        if (produitExist == null) return false;

        _produitRepository.Supprimer(nom);
        return true;
    }
}