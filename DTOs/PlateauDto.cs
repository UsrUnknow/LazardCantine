using LazardCantine.Models;
using LazardCantine.Models.Enums;

namespace LazardCantine.DTOs;

public class PlateauDto
{
    public List<Produit> Produits { get; set; } = new();
}