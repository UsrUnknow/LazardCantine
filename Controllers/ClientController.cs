using Microsoft.AspNetCore.Mvc;
using LazardCantine.DTOs;
using LazardCantine.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace LazardCantine.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientController(ClientService clientService) : ControllerBase
{
    /// <summary>
    /// Ajoute un nouveau client au système.
    /// </summary>
    /// <param name="clientDto">Les données du client à créer, incluant son nom, son type, et son solde initial.</param>
    /// <remarks>
    /// Exemples de types de clients (**ClientType**) :
    /// - 0 : Interne
    /// - 1 : Prestataire
    /// - 2 : VIP
    /// - 3 : Stagiaire
    /// - 4 : Visiteur
    ///
    /// Exemple de requête JSON :
    /// 
    ///     POST /api/Client
    ///     {
    ///         "name": "Jean Dupont",
    ///         "type": 1,
    ///         "balance": 50.00
    ///     }
    /// </remarks>
    /// <returns>Le client nouvellement créé dans le système.</returns>
    /// <response code="201">Client créé avec succès.</response>
    /// <response code="400">Requête invalide ou données manquantes.</response>
    [HttpPost]
    public IActionResult AjouterClient([FromBody] ClientDto clientDto)
    {
        var client = clientService.CreerClient(clientDto);
        return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
    }

    /// <summary>
    /// Récupère les informations d'un client spécifique à l'aide de son identifiant unique.
    /// </summary>
    /// <param name="id">L'identifiant unique du client (GUID).</param>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     GET /api/Client/123e4567-e89b-12d3-a456-426614174000
    /// </remarks>
    /// <returns>
    /// Les détails du client si trouvé, notamment son nom, son type, et son solde actuel.
    /// </returns>
    /// <response code="200">Client trouvé.</response>
    /// <response code="404">Client introuvable.</response>
    [HttpGet("{id}")]
    public IActionResult GetClientById(Guid id)
    {
        var client = clientService.ObtenirClientParId(id);
        if (client == null)
            return NotFound();

        return Ok(client);
    }

    /// <summary>
    /// Liste tous les clients inscrits dans le système.
    /// </summary>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     GET /api/Client
    /// </remarks>
    /// <returns>
    /// Une liste de tous les clients, incluant leurs noms, types, et soldes actuels.
    /// </returns>
    /// <response code="200">Liste des clients retournée avec succès.</response>
    [HttpGet]
    public IActionResult GetTousLesClients()
    {
        var clients = clientService.ObtenirTousLesClients();
        return Ok(clients);
    }

    /// <summary>
    /// Ajoute un montant au solde d'un client spécifique.
    /// </summary>
    /// <param name="id">L'identifiant unique du client (GUID).</param>
    /// <param name="montant">Le montant à ajouter au solde du client.</param>
    /// <remarks>
    /// Exemple de requête :
    /// 
    ///     PUT /api/Client/123e4567-e89b-12d3-a456-426614174000/crediter?montant=100
    /// </remarks>
    /// <returns>Un message confirmant que le solde du client a été mis à jour.</returns>
    /// <response code="200">Compte crédité avec succès.</response>
    /// <response code="404">Client introuvable.</response>
    [HttpPut("{id}/crediter")]
    public IActionResult CrediterCompte(Guid id, [FromQuery] decimal montant)
    {
        if (!clientService.CrediterCompte(id, montant))
            return NotFound();

        return Ok("Compte crédité avec succès.");
    }
}