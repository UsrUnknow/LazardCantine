using System.ComponentModel.DataAnnotations;
using LazardCantine.Models.Enums;

namespace LazardCantine.Models;

public class Produit
{
    [Key] // Définition de la clé primaire
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductType Type { get; set; }
}