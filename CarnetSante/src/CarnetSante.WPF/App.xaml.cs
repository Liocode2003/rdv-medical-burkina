using CarnetSante.Core.Services;
using CarnetSante.Data.Context;
using CarnetSante.Data.Repositories;
using CarnetSante.WPF.Services;
using CarnetSante.WPF.ViewModels.Admin;
using CarnetSante.WPF.ViewModels.Auth;
using CarnetSante.WPF.ViewModels.Dashboard;
using CarnetSante.WPF.ViewModels.Patient;
using CarnetSante.WPF.Views.Admin;
using CarnetSante.WPF.Views.Auth;
using CarnetSante.WPF.Views.Dashboard;
using CarnetSante.WPF.Views.Patient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;

namespace CarnetSante.WPF;

public partial class App : Application
{
    private static IServiceProvider? _serviceProvider;
    private static IConfiguration? _configuration;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _configuration = ChargerConfiguration();

        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        await InitialiserBaseDeDonneesAsync();

        var loginWindow = GetLoginWindow();
        loginWindow.Show();
    }

    /// <summary>
    /// Charge la configuration depuis appsettings.json et appsettings.Production.json (optionnel).
    /// Le mot de passe de chiffrement peut aussi être fourni via la variable d'environnement
    /// CARNETSANTE_DB_PASSWORD.
    /// </summary>
    private static IConfiguration ChargerConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "CARNETSANTE_");

        return builder.Build();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton(_configuration!);

        // ── Base de données SQLite (avec chiffrement optionnel) ──────────
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarnetSante",
            "carnet_sante.db");

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        bool chiffrementActif = _configuration!.GetValue<bool>("Database:ChiffrementActif");
        string motDePasse = ObtenirMotDePasseBdd();

        services.AddDbContext<CarnetSanteDbContext>(options =>
        {
            if (chiffrementActif && !string.IsNullOrWhiteSpace(motDePasse))
            {
                // Connexion SQLCipher avec mot de passe (chiffrement AES-256)
                options.UseSqlite($"Data Source={dbPath};Password={motDePasse}");
            }
            else if (chiffrementActif && string.IsNullOrWhiteSpace(motDePasse))
            {
                // Chiffrement demandé mais pas de mot de passe configuré : erreur fatale
                throw new InvalidOperationException(
                    "Le chiffrement de la base de données est activé mais aucun mot de passe " +
                    "n'est configuré. Définissez 'Database:MotDePasseChiffrement' dans " +
                    "appsettings.Production.json ou la variable d'environnement " +
                    "CARNETSANTE_Database__MotDePasseChiffrement.");
            }
            else
            {
                options.UseSqlite($"Data Source={dbPath}");
            }
        });

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
        services.AddTransient<GestionUtilisateursViewModel>();
        services.AddTransient<JournalAuditViewModel>();

        // ── Vues ─────────────────────────────────────────────
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<PatientWindow>();
        services.AddTransient<GestionUtilisateursWindow>();
        services.AddTransient<JournalAuditWindow>();

        services.AddLogging(builder => builder.AddDebug());
    }

    /// <summary>
    /// Résout le mot de passe de chiffrement par ordre de priorité :
    /// 1. Variable d'environnement CARNETSANTE_Database__MotDePasseChiffrement
    /// 2. appsettings.Production.json → Database:MotDePasseChiffrement
    /// 3. appsettings.json → Database:MotDePasseChiffrement (vide par défaut)
    /// </summary>
    private static string ObtenirMotDePasseBdd()
    {
        return _configuration!.GetValue<string>("Database:MotDePasseChiffrement") ?? string.Empty;
    }

    private static async Task InitialiserBaseDeDonneesAsync()
    {
        using var scope = _serviceProvider!.CreateScope();
        var db          = scope.ServiceProvider.GetRequiredService<CarnetSanteDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        await db.Database.MigrateAsync();

        // Vérifier que l'admin par défaut a un hash BCrypt valide.
        // La migration seed peut contenir un hash généré en dehors de l'appli ;
        // on le régénère à la première exécution si la vérification échoue.
        var admin = await db.Utilisateurs.FirstOrDefaultAsync(u => u.Login == "admin");
        if (admin != null && !authService.VerifierMotDePasse("Admin@2024!", admin.MotDePasseHash))
        {
            admin.MotDePasseHash = authService.HacherMotDePasse("Admin@2024!");
            await db.SaveChangesAsync();
        }
        else if (admin == null)
        {
            // Aucun admin trouvé (base vierge ou seed manqué) : on en crée un
            db.Utilisateurs.Add(new Core.Models.Utilisateur
            {
                Login          = "admin",
                MotDePasseHash = authService.HacherMotDePasse("Admin@2024!"),
                Nom            = "Administrateur",
                Prenom         = "Système",
                Role           = Core.Enums.UserRole.Administrateur,
                EstActif       = true,
                CreatedAt      = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }

    public static LoginWindow GetLoginWindow()
    {
        var loginVm     = _serviceProvider!.GetRequiredService<LoginViewModel>();
        var loginWindow = new LoginWindow(loginVm);

        loginVm.ConnexionReussie += () =>
        {
            var mainVm     = _serviceProvider!.GetRequiredService<MainViewModel>();
            var authService = _serviceProvider!.GetRequiredService<IAuthService>();

            var mainWindow = new MainWindow(
                mainVm,
                () => _serviceProvider!.GetRequiredService<PatientWindow>(),
                () => _serviceProvider!.GetRequiredService<GestionUtilisateursWindow>(),
                () => _serviceProvider!.GetRequiredService<JournalAuditWindow>(),
                authService);

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
