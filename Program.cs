using LazardCantine.Infrastructure;
using LazardCantine.Models;
using LazardCantine.Models.Enums;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    options.IncludeXmlComments(xmlPath);

    options.EnableAnnotations();
});

// Ajouter les dépendances et la base de données
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("CantineDb");
});

async Task SeedDatabaseAsync(AppDbContext context)
{
    if (!await context.Produits.AnyAsync())
    {
        await context.Produits.AddRangeAsync(
            new Produit { Id = 1, Name = "Salade César", Price     = 3.0m, Type = ProductType.Entree },
            new Produit { Id = 2, Name = "Poulet Rôti", Price      = 8.0m, Type = ProductType.Plat },
            new Produit { Id = 3, Name = "Tarte aux Pommes", Price = 2.0m, Type = ProductType.Dessert },
            new Produit { Id = 4, Name = "Eau Plate", Price        = 1.0m, Type = ProductType.Boisson },
            new Produit { Id = 5, Name = "Petit Salade Bar", Price = 4.0m, Type = ProductType.PetitSaladeBar },
            new Produit { Id = 6, Name = "Grand Salade Bar", Price = 6.0m, Type = ProductType.GrandSaladeBar }
        );
    }

    if (!await context.Clients.AnyAsync())
    {
        await context.Clients.AddRangeAsync(
            new Client { Id = Guid.NewGuid(), Name = "Jean Dupont", Balance = 15.0m, ClientType = ClientType.Interne },
            new Client { Id = Guid.NewGuid(), Name = "Claire Martin", Balance = 40.0m, ClientType = ClientType.Prestataire },
            new Client { Id = Guid.NewGuid(), Name = "Daniel Morel", Balance = 100.0m, ClientType = ClientType.VIP },
            new Client { Id = Guid.NewGuid(), Name = "Lisa Dupuis", Balance = 12.0m, ClientType = ClientType.Stagiaire },
            new Client { Id = Guid.NewGuid(), Name = "Alex Lambert", Balance = 5.0m, ClientType = ClientType.Visiteur }
        );
    }

    await context.SaveChangesAsync();
}

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();  

var app = builder.Build();

// Créer un scope pour résoudre les services (comme AppDbContext)
using (var scope = app.Services.CreateScope())
{
    // Récupérer l'instance de AppDbContext depuis le container d'injection
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Appeler la fonction SeedDatabase pour alimenter les données initiales
    await SeedDatabaseAsync(context);
}

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        logger.LogInformation("Seeding database...");
        await SeedDatabaseAsync(context);
        logger.LogInformation("Database seeding completed.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database seeding.");
        throw;
    }
}

// Configuration de l'API REST
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();