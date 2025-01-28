using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;
using LazardCantine.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LazardCantine.Tests;

public class PlateauServiceTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Produit>> _mockProduits;
    private readonly ClientRepository _clientRepository;
    private readonly IPlateauService _plateauService;

    public PlateauServiceTests()
    {
        // Préparer une collection de données simulées
        var produits = new List<Produit>
        {
            new Produit { Id = 1, Name = "Entrée", Price  = 2.0m, Type = ProductType.Entree },
            new Produit { Id = 2, Name = "Plat", Price    = 5.0m, Type = ProductType.Plat },
            new Produit { Id = 3, Name = "Dessert", Price = 2.0m, Type = ProductType.Dessert },
            new Produit { Id = 4, Name = "Pain", Price    = 1.0m, Type = ProductType.Pain }
        }.AsQueryable();

        // Configurer un mock d'IQueryable<Produit>
        _mockProduits = new Mock<DbSet<Produit>>();
        _mockProduits.As<IQueryable<Produit>>().Setup(m => m.Provider).Returns(produits.Provider);
        _mockProduits.As<IQueryable<Produit>>().Setup(m => m.Expression).Returns(produits.Expression);
        _mockProduits.As<IQueryable<Produit>>().Setup(m => m.ElementType).Returns(produits.ElementType);
        _mockProduits.As<IQueryable<Produit>>().Setup(m => m.GetEnumerator()).Returns(() => produits.GetEnumerator());

        // Simuler l'ajout et le retrait de données
        _mockProduits.Setup(m => m.Add(It.IsAny<Produit>())).Callback<Produit>(p => produits.ToList().Add(p));
        _mockProduits.Setup(m => m.Remove(It.IsAny<Produit>())).Callback<Produit>(p => produits.ToList().Remove(p));

        // Mock du DbContext pour retourner DbSet<Produit>
        _mockContext = new Mock<AppDbContext>();
        _mockContext.Setup(c => c.Produits).Returns(_mockProduits.Object);

        // Initialiser les dépendances
        _clientRepository = new ClientRepository();
        _plateauService   = new PlateauService(_clientRepository);
    }
    
    [Fact]
    public void Test_PlateauAvecPrixFixe()
    {
        // Arrange
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = "Client Interne",
            ClientType = ClientType.Interne,
            Balance    = 10.0m
        };

        _clientRepository.Ajouter(client);

        var plateau = new PlateauDto
        {
            Produits = new List<Produit>
            {
                new Produit { Id = 1, Name = "Entrée", Type  = ProductType.Entree, Price  = 2.0m },
                new Produit { Id = 2, Name = "Plat", Type    = ProductType.Plat, Price    = 5.0m },
                new Produit { Id = 3, Name = "Dessert", Type = ProductType.Dessert, Price = 2.0m },
                new Produit { Id = 4, Name = "Pain", Type    = ProductType.Pain, Price    = 1.0m }
            }
        };

        // Act
        var result = _plateauService.PayerPlateau(client.Id, plateau);

        // Assert
        Assert.True(result.Succes);
        Assert.Equal(10.0m, result.Ticket.Total);
        Assert.Equal(2.5m, result.Ticket.TotalFinal);
        Assert.Equal(7.5m, client.Balance);
    }
    
    [Fact]
    public void Test_PlatAvecSupplements()
    {
        // Arrange
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = "Client Prestataire",
            ClientType = ClientType.Prestataire,
            Balance    = 20.0m
        };

        _clientRepository.Ajouter(client);

        var plateau = new PlateauDto
        {
            Produits = new List<Produit>
            {
                new Produit { Id = 1, Name = "Entrée", Type = ProductType.Entree, Price = 2.0m },
                new Produit { Id = 2, Name = "Plat", Type = ProductType.Plat, Price = 5.0m },
                new Produit { Id = 3, Name = "Dessert", Type = ProductType.Dessert, Price = 2.0m },
                new Produit { Id = 5, Name = "Boisson", Type = ProductType.Boisson, Price = 1.0m },               // Supplément
                new Produit { Id = 6, Name = "Grand Salade Bar", Type = ProductType.GrandSaladeBar, Price = 6.0m } // Supplément
            }
        };

        // Act
        var result = _plateauService.PayerPlateau(client.Id, plateau);

        // Assert
        Assert.True(result.Succes);
        Assert.Equal(16.0m, result.Ticket.Total);
        Assert.Equal(10.0m, result.Ticket.TotalFinal);
        Assert.Equal(10.0m, client.Balance);
    }
    
    [Fact]
    public void Test_SoldeInsuffisant_Visiteur()
    {
        // Arrange
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = "Client Visiteur",
            ClientType = ClientType.Visiteur,
            Balance    = 5.0m // Solde insuffisant
        };

        _clientRepository.Ajouter(client);

        var plateau = new PlateauDto
        {
            Produits = new List<Produit>
            {
                new Produit { Id = 1, Name = "Entrée", Type = ProductType.Entree, Price = 2.0m },
                new Produit { Id = 2, Name = "Plat", Type   = ProductType.Plat, Price   = 5.0m }
            }
        };

        // Act
        var result = _plateauService.PayerPlateau(client.Id, plateau);

        // Assert
        Assert.False(result.Succes);        // Paiement refusé
        Assert.Equal("Solde insuffisant", result.Message);
        Assert.Equal(5.0m, client.Balance); // Solde inchangé
    }
    
    [Fact]
    public void Test_SoldeInsuffisant_InterneDecouvert()
    {
        // Arrange
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = "Client Interne",
            ClientType = ClientType.Interne,
            Balance    = 2.0m // Solde insuffisant, mais découvert autorisé
        };

        _clientRepository.Ajouter(client);

        var plateau = new PlateauDto
        {
            Produits = new List<Produit>
            {
                new Produit { Id = 1, Name = "Entrée", Type  = ProductType.Entree, Price  = 2.0m },
                new Produit { Id = 2, Name = "Plat", Type    = ProductType.Plat, Price    = 5.0m },
                new Produit { Id = 3, Name = "Dessert", Type = ProductType.Dessert, Price = 2.0m },
                new Produit { Id = 4, Name = "Pain", Type    = ProductType.Pain, Price    = 1.0m }
            }
        };

        // Act
        var result = _plateauService.PayerPlateau(client.Id, plateau);

        // Assert
        Assert.True(result.Succes);
        Assert.Equal(10.0m, result.Ticket.Total);
        Assert.Equal(2.5m, result.Ticket.TotalFinal);
        Assert.Equal(-0.5m, client.Balance);
    }
    
    [Fact]
    public void Test_PlateauIncomplet()
    {
        // Arrange
        var client = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = "Client Stagiaire",
            ClientType = ClientType.Stagiaire,
            Balance    = 15.0m
        };

        _clientRepository.Ajouter(client);

        var plateau = new PlateauDto
        {
            Produits = new List<Produit>
            {
                new Produit { Id = 1, Name = "Entrée", Type = ProductType.Entree, Price = 2.0m },
                new Produit { Id = 2, Name = "Plat", Type   = ProductType.Plat, Price   = 5.0m }
            }
        };

        // Act
        var result = _plateauService.PayerPlateau(client.Id, plateau);

        // Assert
        Assert.True(result.Succes);
        Assert.Equal(7.0m, result.Ticket.Total);
        Assert.Equal(0.0m, result.Ticket.TotalFinal);
        Assert.Equal(15.0m, client.Balance);
    }
}