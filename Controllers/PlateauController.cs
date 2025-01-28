using LazardCantine.DTOs;
using LazardCantine.Services;
using Microsoft.AspNetCore.Mvc;

namespace LazardCantine.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlateauController : ControllerBase
{
    private readonly IPlateauService _plateauService;

    public PlateauController(IPlateauService plateauService)
    {
        _plateauService = plateauService;
    }

    /// <summary>
    /// Effectue le paiement pour un plateau spécifique d'un client.
    /// </summary>
    /// <param name="clientId">L'identifiant unique du client (GUID).</param>
    /// <param name="plateau">Une liste des produits contenus dans le plateau.</param>
    /// <remarks>
    /// **Exemple de structure JSON pour "plateau"** :
    /// 
    ///     {
    ///         "produits": [
    ///             { "name": "Sandwich", "type": "Plat", "prix": 5.50 },
    ///             { "name": "Boisson", "type": "Boisson", "prix": 1.50 }
    ///         ]
    ///     }
    /// 
    /// - Le calcul du prix total du plateau dépend des règles métier (ex. : prix fixe pour un plateau complet).
    ///
    /// **Types de produits** :
    /// - Entrée 
    /// - Plat
    /// - Dessert
    /// - Pain
    /// - Boisson
    /// </remarks>
    /// <returns>Un ticket contenant les détails du paiement.</returns>
    /// <response code="200">Paiement réussi, ticket retourné avec succès.</response>
    /// <response code="400">Échec du paiement (solde insuffisant, client introuvable, etc.).</response>
    [HttpPost("{clientId}/payer")]
    public IActionResult FinaliserPaiement(Guid clientId, [FromBody] PlateauDto plateau)
    {
        var result = _plateauService.PayerPlateau(clientId, plateau);
        if (!result.Succes)
            return BadRequest(result.Message);

        return Ok(result.Ticket);
    }
}