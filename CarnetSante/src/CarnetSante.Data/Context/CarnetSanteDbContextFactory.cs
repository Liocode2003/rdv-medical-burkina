using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarnetSante.Data.Context;

/// <summary>
/// Factory utilisée par les outils EF Core (dotnet ef) au moment de la conception pour les migrations.
/// Permet d'exécuter : dotnet ef migrations add / dotnet ef database update
///
/// IMPORTANT : cette factory utilise une base de données sans chiffrement car elle n'est
/// employée que lors du développement (génération de migrations). Elle ne doit JAMAIS
/// être utilisée en production.
/// </summary>
public class CarnetSanteDbContextFactory : IDesignTimeDbContextFactory<CarnetSanteDbContext>
{
    public CarnetSanteDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarnetSanteDbContext>();

        // Utilise le répertoire courant du projet Data pour les migrations design-time.
        // Cela évite d'écrire dans le répertoire temporaire système (plus prévisible).
        var projectDir = Path.GetDirectoryName(
            typeof(CarnetSanteDbContextFactory).Assembly.Location)!;
        var dbPath = Path.Combine(projectDir, "carnet_sante_design.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new CarnetSanteDbContext(optionsBuilder.Options);
    }
}
