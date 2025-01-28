using LazardCantine.Models.Enums;

namespace LazardCantine.Infrastructure;

using Models;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Produit> Produits { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Produit>()
                    .Property(p => p.Id)
                    .ValueGeneratedOnAdd();

        // Ajout des produits
        modelBuilder.Entity<Produit>().HasData(
            new Produit { Id = 1, Name = "Salade César", Price     = 3.0m, Type = ProductType.Entree },
            new Produit { Id = 2, Name = "Poulet Rôti", Price      = 8.0m, Type = ProductType.Plat },
            new Produit { Id = 3, Name = "Tarte aux Pommes", Price = 2.0m, Type = ProductType.Dessert },
            new Produit { Id = 4, Name = "Eau Plate", Price        = 1.0m, Type = ProductType.Boisson },
            new Produit { Id = 5, Name = "Petit Salade Bar", Price = 4.0m, Type = ProductType.PetitSaladeBar },
            new Produit { Id = 6, Name = "Grand Salade Bar", Price = 6.0m, Type = ProductType.GrandSaladeBar }
        );

        // Ajout des clients
        modelBuilder.Entity<Client>().HasData(
            new Client { Id = Guid.NewGuid(), Name = "Jean Dupont", Balance = 15.0m, ClientType = ClientType.Interne },
            new Client { Id = Guid.NewGuid(), Name = "Claire Martin", Balance = 40.0m, ClientType = ClientType.Prestataire },
            new Client { Id = Guid.NewGuid(), Name = "Daniel Morel", Balance = 100.0m, ClientType = ClientType.VIP },
            new Client { Id = Guid.NewGuid(), Name = "Lisa Dupuis", Balance = 12.0m, ClientType = ClientType.Stagiaire },
            new Client { Id = Guid.NewGuid(), Name = "Alex Lambert", Balance = 5.0m, ClientType = ClientType.Visiteur }
        );
    }
    
    public static void SeedDatabase(AppDbContext context)
    {
        if (!context.Produits.Any())
        {
            context.Produits.AddRange(
                new Produit { Name = "Salade César", Price     = 3.0m, Type = ProductType.Entree },
                new Produit { Name = "Poulet Rôti", Price      = 8.0m, Type = ProductType.Plat },
                new Produit { Name = "Tarte aux Pommes", Price = 2.0m, Type = ProductType.Dessert },
                new Produit { Name = "Eau Plate", Price        = 1.0m, Type = ProductType.Boisson },
                new Produit { Name = "Petit Salade Bar", Price = 4.0m, Type = ProductType.PetitSaladeBar },
                new Produit { Name = "Grand Salade Bar", Price = 6.0m, Type = ProductType.GrandSaladeBar }
            );
        }

        if (!context.Clients.Any())
        {
            context.Clients.AddRange(
                new Client { Id = Guid.NewGuid(), Name = "Jean Dupont", Balance = 15.0m, ClientType = ClientType.Interne },
                new Client { Id = Guid.NewGuid(), Name = "Claire Martin", Balance = 40.0m, ClientType = ClientType.Prestataire },
                new Client { Id = Guid.NewGuid(), Name = "Daniel Morel", Balance = 100.0m, ClientType = ClientType.VIP },
                new Client { Id = Guid.NewGuid(), Name = "Lisa Dupuis", Balance = 12.0m, ClientType = ClientType.Stagiaire },
                new Client { Id = Guid.NewGuid(), Name = "Alex Lambert", Balance = 5.0m, ClientType = ClientType.Visiteur }
            );
        }

        context.SaveChanges();
    }
}