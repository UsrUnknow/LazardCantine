using LazardCantine.DTOs;
using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;
using LazardCantine.Services;
using Moq;
using Xunit;

namespace LazardCantine.Tests;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _clientService = new ClientService(_mockClientRepository.Object);
    }
    
    [Fact]
    public void CreerClient_DoitAjouterClient()
    {
        // Arrange
        var clientDto = new ClientDto
        {
            Name    = "Client Test",
            Type    = ClientType.Interne,
            Balance = 20.0m
        };

        var addedClient = new Client
        {
            Id         = Guid.NewGuid(),
            Name       = clientDto.Name,
            ClientType = clientDto.Type,
            Balance    = clientDto.Balance
        };

        _mockClientRepository
            .Setup(repo => repo.Ajouter(It.IsAny<Client>())) 
            .Callback<Client>(c => addedClient = c);

        // Act
        var result = _clientService.CreerClient(clientDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientDto.Name, result.Name);
        Assert.Equal(clientDto.Type, result.ClientType);
        Assert.Equal(clientDto.Balance, result.Balance);
        _mockClientRepository.Verify(repo => repo.Ajouter(It.Is<Client>(c => c.Name == clientDto.Name)), Times.Once);
    }


    [Fact]
    public void ObtenirClientParId_RetourneClientSiExiste()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            Name = "Client Test",
            ClientType = ClientType.Interne,
            Balance = 50.0m
        };

        _mockClientRepository
            .Setup(repo => repo.ObtenirParId(clientId))
            .Returns(client);

        // Act
        var result = _clientService.ObtenirClientParId(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(client.Id, result?.Id);
        Assert.Equal(client.Name, result?.Name);
    }

    [Fact]
    public void CrediterCompte_CrediteAvecSucces()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            Name = "Client Test",
            ClientType = ClientType.Interne,
            Balance = 10.0m
        };

        _mockClientRepository
            .Setup(repo => repo.ObtenirParId(clientId))
            .Returns(client);

        // Act
        var success = _clientService.CrediterCompte(clientId, 5.0m);

        // Assert
        Assert.True(success);
        Assert.Equal(15.0m, client.Balance);
        _mockClientRepository.Verify(repo => repo.MettreAJour(client), Times.Once);
    }

    [Fact]
    public void CrediterCompte_ClientInexistantRetourneFaux()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        _mockClientRepository
            .Setup(repo => repo.ObtenirParId(clientId))
            .Returns((Client?)null);

        // Act
        var success = _clientService.CrediterCompte(clientId, 5.0m);

        // Assert
        Assert.False(success);
        _mockClientRepository.Verify(repo => repo.MettreAJour(It.IsAny<Client>()), Times.Never);
    }
}