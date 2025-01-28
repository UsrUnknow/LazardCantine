using LazardCantine.Models;

namespace LazardCantine.Infrastructure
{
    public interface IProduitRepository
    {
        IEnumerable<Produit> ObtenirTous();
        Produit? ObtenirParNom(string nom);
        void Ajouter(Produit produit);
        void Supprimer(string nom);
    }
}