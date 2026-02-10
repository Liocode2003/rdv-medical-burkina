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

        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        await InitialiserBaseDeDonneesAsync();

        AfficherLogin();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarnetSante",
            "carnet_sante.db");

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        // DbContext en Singleton pour l'application desktop (1 utilisateur, pas de concurrence)
        services.AddDbContext<CarnetSanteDbContext>(
            options => options.UseSqlite($"Data Source={dbPath}"),
            ServiceLifetime.Singleton);

        // Repositories et services en Singleton (partagé sur toute la durée de l'application)
        services.AddSingleton<IUtilisateurRepository, UtilisateurRepository>();
        services.AddSingleton<IPatientRepository, PatientRepository>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IAuditService, AuditService>();
        services.AddSingleton<IPatientService, PatientService>();
        services.AddSingleton<IPdfService, PdfService>();

        // ViewModels en Transient (nouvelle instance à chaque fenêtre)
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<PatientViewModel>();

        services.AddLogging(builder => builder.AddDebug());
    }

    private static async Task InitialiserBaseDeDonneesAsync()
    {
        var db = _serviceProvider!.GetRequiredService<CarnetSanteDbContext>();
        await db.Database.MigrateAsync();
        await db.SeedAdminAsync(); // Crée l'admin si premier démarrage
    }

    /// <summary>
    /// Affiche la fenêtre de connexion. Appelé au démarrage et après déconnexion.
    /// </summary>
    public static void AfficherLogin()
    {
        var loginVm = _serviceProvider!.GetRequiredService<LoginViewModel>();
        var loginWindow = new LoginWindow(loginVm);

        loginVm.ConnexionReussie += () =>
        {
            var mainVm = _serviceProvider!.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainVm, CreerPatientWindow);
            mainWindow.Show();
        };

        loginWindow.Show();
    }

    private static PatientWindow CreerPatientWindow()
    {
        var vm = _serviceProvider!.GetRequiredService<PatientViewModel>();
        return new PatientWindow(vm);
    }

    // Méthode conservée pour compatibilité avec MainWindow.xaml.cs
    public static LoginWindow GetLoginWindow()
    {
        var loginVm = _serviceProvider!.GetRequiredService<LoginViewModel>();
        var loginWindow = new LoginWindow(loginVm);

        loginVm.ConnexionReussie += () =>
        {
            var mainVm = _serviceProvider!.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainVm, CreerPatientWindow);
            mainWindow.Show();
        };

        return loginWindow;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        (_serviceProvider as IDisposable)?.Dispose();
        base.OnExit(e);
    }
}
