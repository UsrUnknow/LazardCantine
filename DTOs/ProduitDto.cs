namespace LazardCantine.DTOs;

public class ProduitDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; } // Nous utiliserons ici une conversion de l'enum en string pour plus de lisibilité en front-end.
}