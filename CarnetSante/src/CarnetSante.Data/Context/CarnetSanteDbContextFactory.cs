using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarnetSante.Data.Context;

/// <summary>
/// Factory utilisée par les outils EF Core (dotnet ef) au moment de la conception pour les migrations.
/// Permet d'exécuter : dotnet ef migrations add / dotnet ef database update
/// </summary>
public class CarnetSanteDbContextFactory : IDesignTimeDbContextFactory<CarnetSanteDbContext>
{
    public CarnetSanteDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarnetSanteDbContext>();

        // Chemin temporaire utilisé uniquement lors de la génération des migrations
        var dbPath = Path.Combine(Path.GetTempPath(), "carnet_sante_design.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new CarnetSanteDbContext(optionsBuilder.Options);
    }
}
