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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;

namespace CarnetSante.WPF;

public partial class App : Application
{
    private static IServiceProvider? _serviceProvider;
    private static IServiceScope? _appScope;
    private static IConfiguration? _configuration;

    // Toujours résoudre les services depuis le scope applicatif pour que
    // IAuthService (Scoped) soit la même instance dans tout l'application.
    private static IServiceProvider AppServices => _appScope!.ServiceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _configuration = ChargerConfiguration();

        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
        _appScope = _serviceProvider.CreateScope();

        await InitialiserBaseDeDonneesAsync();

        bool connecte = await TenterAutoConnexionAsync();
        if (connecte)
            OuvrirFenetrePrincipale();
        else
            OuvrirFenetreDeConnexion();
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

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Impossible de créer le répertoire de la base de données : {Path.GetDirectoryName(dbPath)}\n" +
                $"Vérifiez les droits d'écriture sur le dossier AppData\\Local.", ex);
        }

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

        // ── Vues ─────────────────────────────────────────────
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<PatientWindow>();

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
        var db = scope.ServiceProvider.GetRequiredService<CarnetSanteDbContext>();
        try
        {
            await db.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Échec de l'initialisation de la base de données. " +
                "Vérifiez que le fichier n'est pas verrouillé par un autre processus " +
                "et que vous disposez des droits nécessaires.", ex);
        }
    }

    /// <summary>
    /// Résout le chemin du dossier d'export PDF en développant les variables d'environnement
    /// Windows (ex. %USERPROFILE%) présentes dans la configuration.
    /// </summary>
    public static string ObtenirDossierExportPdf()
    {
        var chemin = _configuration!.GetValue<string>("PDF:DossierExport")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CarnetSante", "Exports");

        return Environment.ExpandEnvironmentVariables(chemin);
    }

    /// <summary>
    /// Tente une connexion automatique avec le compte admin par défaut.
    /// Retourne true si la connexion a réussi, false sinon.
    /// En cas d'échec, affiche la fenêtre de login comme solution de repli.
    /// </summary>
    private static async Task<bool> TenterAutoConnexionAsync()
    {
        string? erreur = null;
        try
        {
            var authService = AppServices.GetRequiredService<IAuthService>();
            var utilisateur = await authService.ConnecterAsync("admin", "Admin@2024!");
            if (utilisateur != null)
                return true;
            // ConnecterAsync a retourné null → mot de passe incorrect ou utilisateur introuvable
            erreur = "Connexion automatique échouée (identifiants invalides).";
        }
        catch (InvalidOperationException ex)
        {
            erreur = ex.Message; // compte bloqué
        }
        catch (Exception ex)
        {
            erreur = $"Erreur inattendue lors de la connexion automatique :\n{ex.Message}";
        }

        // Connexion auto impossible → afficher la fenêtre de login
        MessageBox.Show(
            $"{erreur}\n\nVeuillez vous connecter manuellement.",
            "Connexion automatique impossible",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

        return false;
    }

    private static void OuvrirFenetrePrincipale()
    {
        var mainVm = AppServices.GetRequiredService<MainViewModel>();
        var mainWindow = new MainWindow(mainVm,
            () => AppServices.GetRequiredService<PatientWindow>());

        mainWindow.Show();
    }

    private static void OuvrirFenetreDeConnexion()
    {
        var loginWindow = GetLoginWindow();
        loginWindow.Show();
    }

    public static LoginWindow GetLoginWindow()
    {
        var loginVm = AppServices.GetRequiredService<LoginViewModel>();
        var loginWindow = new LoginWindow(loginVm);

        loginVm.ConnexionReussie += () =>
        {
            var mainVm = AppServices.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainVm,
                () => AppServices.GetRequiredService<PatientWindow>());
            mainWindow.Show();
            loginWindow.Close();
        };

        return loginWindow;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _appScope?.Dispose();
        (_serviceProvider as IDisposable)?.Dispose();
        base.OnExit(e);
    }
}
