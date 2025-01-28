using LazardCantine.DTOs;
using LazardCantine.Services;
using Microsoft.AspNetCore.Mvc;

namespace LazardCantine.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProduitController : ControllerBase
{
    private readonly ProduitService _produitService;

    public ProduitController(ProduitService produitService)
    {
        _produitService = produitService;
    }

    /// <summary>
    /// Récupère tous les produits disponibles dans le système.
    /// </summary>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     GET /api/Produit
    /// 
    /// Les produits retournés incluent leurs noms, prix, et types.
    /// </remarks>
    /// <returns>Une liste de tous les produits disponibles avec leurs détails.</returns>
    /// <response code="200">Liste des produits retournée avec succès.</response>
    [HttpGet]
    public IActionResult GetTousLesProduits()
    {
        var produits = _produitService.GetTousLesProduits();
        return Ok(produits);
    }
    
    /// <summary>
    /// Récupère un produit spécifique en fonction de son nom.
    /// </summary>
    /// <param name="nom">Le nom complet du produit souhaité.</param>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     GET /api/Produit/Salade César
    /// </remarks>
    /// <returns>Un produit correspondant au nom demandé, avec son type et son prix.</returns>
    /// <response code="200">Produit trouvé et retourné avec succès.</response>
    /// <response code="404">Produit introuvable.</response>
    [HttpGet("{nom}")]
    public IActionResult GetProduitParNom(string nom)
    {
        var produit = _produitService.GetProduitParNom(nom);
        if (produit == null)
            return NotFound(new { Message = $"Aucun produit trouvé avec le nom '{nom}'." });

        return Ok(produit);
    }

    /// <summary>
    /// Ajoute un nouveau produit dans le système.
    /// </summary>
    /// <param name="produit">Les informations du produit à ajouter incluant son nom, prix, et type.</param>
    /// <remarks>
    /// **Types de produits acceptés** :
    /// - Entree
    /// - Plat
    /// - Dessert
    /// - Pain
    /// - Boisson
    ///
    /// Exemple de requête :
    /// 
    ///     POST /api/Produit
    ///     {
    ///         "name": "Café",
    ///         "price": 1.5,
    ///         "type": "Boisson"
    ///     }
    /// </remarks>
    /// <returns>Le produit nouvellement ajouté.</returns>
    /// <response code="201">Produit ajouté avec succès.</response>
    /// <response code="400">Données invalides ou requête incorrecte.</response>
    [HttpPost]
    public IActionResult AjouterProduit([FromBody] ProduitDto produit)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _produitService.AjouterProduit(produit);
        return CreatedAtAction(nameof(GetProduitParNom), new { nom = produit.Name }, produit);
    }

    /// <summary>
    /// Supprime un produit existant en fonction de son nom.
    /// </summary>
    /// <param name="nom">Le nom complet du produit à supprimer.</param>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     DELETE /api/Produit/Salade César
    /// 
    /// Supprime le produit correspondant au nom demandé.
    /// </remarks>
    /// <response code="204">Produit supprimé avec succès.</response>
    /// <response code="404">Produit introuvable.</response>
    [HttpDelete("{nom}")]
    public IActionResult SupprimerProduit(string nom)
    {
        var produit = _produitService.GetProduitParNom(nom);
        if (produit == null)
            return NotFound(new { Message = $"Aucun produit trouvé avec le nom '{nom}' pour suppression." });

        _produitService.SupprimerProduit(nom);
        return NoContent();
    }
}