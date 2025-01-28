using LazardCantine.Models;

namespace LazardCantine.Infrastructure;

public class ProduitRepository : IProduitRepository
{
    private readonly AppDbContext _context;

    public ProduitRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Produit> ObtenirTous()
    {
        return _context.Produits.ToList();
    }

    public Produit? ObtenirParNom(string nom)
    {
        return _context.Produits.FirstOrDefault(p => p.Name.Equals(nom, StringComparison.OrdinalIgnoreCase));
    }

    public void Ajouter(Produit produit)
    {
        _context.Produits.Add(produit);
        _context.SaveChanges();
    }

    public void Supprimer(string nom)
    {
        var produit = ObtenirParNom(nom);
        if (produit != null)
        {
            _context.Produits.Remove(produit);
            _context.SaveChanges();
        }
    }
}