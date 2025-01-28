using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;
using LazardCantine.Services;
using Moq;
using Xunit;

namespace LazardCantine.Tests;

public class ProduitServiceTests
{
    private readonly Mock<IProduitRepository> _mockProduitRepository;
    private readonly ProduitService _produitService;

    public ProduitServiceTests()
    {
        _mockProduitRepository = new Mock<IProduitRepository>();
        _produitService = new ProduitService(_mockProduitRepository.Object);
    }

    [Fact]
    public void GetTousLesProduits_RetourneListeDeDTOs()
    {
        // Arrange
        var produits = new List<Produit>
        {
            new Produit { Name = "Produit1", Price = 2.0m, Type = ProductType.Entree },
            new Produit { Name = "Produit2", Price = 5.0m, Type = ProductType.Plat }
        };

        _mockProduitRepository
            .Setup(repo => repo.ObtenirTous())
            .Returns(produits);

        // Act
        var result = _produitService.GetTousLesProduits();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(produits.Count, result.Count());
        Assert.Contains(result, dto => dto.Name == "Produit1" && dto.Type == ProductType.Entree.ToString());
    }

    [Fact]
    public void GetProduitParNom_RetourneProduitDTO()
    {
        // Arrange
        var produit = new Produit
        {
            Name = "Produit Test",
            Price = 3.0m,
            Type = ProductType.Plat
        };

        _mockProduitRepository
            .Setup(repo => repo.ObtenirParNom("Produit Test"))
            .Returns(produit);

        // Act
        var result = _produitService.GetProduitParNom("Produit Test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produit.Name, result?.Name);
        Assert.Equal(produit.Price, result?.Price);
    }

    [Fact]
    public void GetProduitParNom_RetourneNullSiProduitInexistant()
    {
        // Arrange
        _mockProduitRepository
            .Setup(repo => repo.ObtenirParNom("Produit Inexistant"))
            .Returns((Produit?)null);

        // Act
        var result = _produitService.GetProduitParNom("Produit Inexistant");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void AjouterProduit_AppelleRepositoryAjouter()
    {
        // Arrange
        var produitDto = new ProduitDto
        {
            Name  = "Nouveau Produit",
            Price = 4.0m,
            Type  = ProductType.Dessert.ToString() // Type est en string dans le DTO
        };

        Produit produitAjoute = null; // Pour capturer l'objet ajouté

        _mockProduitRepository
            .Setup(repo => repo.Ajouter(It.IsAny<Produit>()))
            .Callback<Produit>(p => produitAjoute = p);

        // Act
        _produitService.AjouterProduit(produitDto);

        // Assert
        Assert.NotNull(produitAjoute); // Vérifie qu'un objet a bien été transmis
        Assert.Equal(produitDto.Name, produitAjoute.Name); // Le nom doit correspondre
        Assert.Equal(produitDto.Price, produitAjoute.Price); // Le prix doit correspondre
        Assert.Equal(ProductType.Dessert, produitAjoute.Type); // Vérifie la conversion de l'enum
        _mockProduitRepository.Verify(repo => repo.Ajouter(It.IsAny<Produit>()), Times.Once); // Vérifie que le dépôt est appelé une seule fois
    }

    [Fact]
    public void SupprimerProduit_SupprimeProduitSiExistant()
    {
        // Arrange
        var produit = new Produit
        {
            Name = "Produit à supprimer"
        };

        _mockProduitRepository
            .Setup(repo => repo.ObtenirParNom("Produit à supprimer"))
            .Returns(produit);

        // Act
        var success = _produitService.SupprimerProduit("Produit à supprimer");

        // Assert
        Assert.True(success);
        _mockProduitRepository.Verify(repo => repo.Supprimer("Produit à supprimer"), Times.Once);
    }

    [Fact]
    public void SupprimerProduit_RetourneFauxSiProduitInexistant()
    {
        // Arrange
        _mockProduitRepository
            .Setup(repo => repo.ObtenirParNom("Inexistant"))
            .Returns((Produit?)null);

        // Act
        var success = _produitService.SupprimerProduit("Inexistant");

        // Assert
        Assert.False(success);
        _mockProduitRepository.Verify(repo => repo.Supprimer(It.IsAny<string>()), Times.Never);
    }
}