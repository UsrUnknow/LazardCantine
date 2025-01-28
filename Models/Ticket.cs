namespace LazardCantine.Models;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Now;
    public string ClientName { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;

    public List<Produit> Produits { get; set; } = new();

    public decimal Total { get; set; }
    public decimal Reduction { get; set; }
    public decimal TotalFinal { get; set; }
}