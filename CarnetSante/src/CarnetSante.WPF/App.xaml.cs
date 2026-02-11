using CarnetSante.Data.Context;
using CarnetSante.Data.Repositories;
using CarnetSante.Core.Interfaces;
using CarnetSante.WPF.ViewModels;
using CarnetSante.WPF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace CarnetSante.WPF;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        // Ensure database exists and is migrated
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Database
        var dbFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarnetSante");
        Directory.CreateDirectory(dbFolder);
        var dbPath = Path.Combine(dbFolder, "carnet_sante.db");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"),
            ServiceLifetime.Transient);

        // Repositories
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IConstanteRepository, ConstanteRepository>();
        services.AddTransient<IEtatCivilRepository, EtatCivilRepository>();
        services.AddTransient<IContactUrgenceRepository, ContactUrgenceRepository>();
        services.AddTransient<IExamenIncorporationRepository, ExamenIncorporationRepository>();
        services.AddTransient<IOperationMedicaleRepository, OperationMedicaleRepository>();
        services.AddTransient<IVaccinationRepository, VaccinationRepository>();
        services.AddTransient<IVisiteSanitaireRepository, VisiteSanitaireRepository>();
        services.AddTransient<IIndisponibiliteRepository, IndisponibiliteRepository>();
        services.AddTransient<ICertificatMedicalRepository, CertificatMedicalRepository>();
        services.AddTransient<IDecisionReformeRepository, DecisionReformeRepository>();
        services.AddTransient<IControleFinServiceRepository, ControleFinServiceRepository>();

        // Services
        services.AddTransient<IPdfService, PdfService>();
        services.AddTransient<INumeroCarnetService, NumeroCarnetService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<PatientListViewModel>();
        services.AddTransient<PatientDetailViewModel>();
        services.AddTransient<DashboardViewModel>();
    }
}
