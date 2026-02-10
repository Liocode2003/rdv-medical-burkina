using CarnetSante.Core.Services;
using CarnetSante.Data.Context;
using CarnetSante.Data.Repositories;
using CarnetSante.WPF.Services;
using CarnetSante.WPF.ViewModels.Auth;
using CarnetSante.WPF.ViewModels.Dashboard;
using CarnetSante.WPF.ViewModels.Patient;
using CarnetSante.WPF.Views.Auth;
using CarnetSante.WPF.Views.Dashboard;
using CarnetSante.WPF.Views.Patient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;

namespace CarnetSante.WPF;

public partial class App : Application
{
    private static IServiceProvider? _serviceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Configuration de l'injection de dépendances
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // Initialisation / migration de la base de données
        await InitialiserBaseDeDonneesAsync();

        // Afficher la fenêtre de connexion
        var loginWindow = GetLoginWindow();
        loginWindow.Show();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // ── Base de données SQLite ───────────────────────────
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarnetSante",
            "carnet_sante.db");

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        services.AddDbContext<CarnetSanteDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // ── Repositories ─────────────────────────────────────
        services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();

        // ── Services métier ──────────────────────────────────
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IPdfService, PdfService>();

        // ── ViewModels ───────────────────────────────────────
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<PatientViewModel>();

        // ── Vues ─────────────────────────────────────────────
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<PatientWindow>();

        // Logging
        services.AddLogging(builder => builder.AddDebug());
    }

    private static async Task InitialiserBaseDeDonneesAsync()
    {
        using var scope = _serviceProvider!.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarnetSanteDbContext>();
        await db.Database.MigrateAsync();
    }

    /// <summary>
    /// Crée et configure la fenêtre de login (point d'entrée après déconnexion aussi).
    /// </summary>
    public static LoginWindow GetLoginWindow()
    {
        var loginVm = _serviceProvider!.GetRequiredService<LoginViewModel>();
        var loginWindow = new LoginWindow(loginVm);

        loginVm.ConnexionReussie += () =>
        {
            var mainVm = _serviceProvider!.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainVm,
                () => _serviceProvider!.GetRequiredService<PatientWindow>());
            mainWindow.Show();
            loginWindow.Close();
        };

        return loginWindow;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        (_serviceProvider as IDisposable)?.Dispose();
        base.OnExit(e);
    }
}
