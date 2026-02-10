using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using CarnetSante.Data.Repositories;
using System.Collections.ObjectModel;

namespace CarnetSante.WPF.ViewModels.Admin;

/// <summary>
/// ViewModel de gestion des comptes utilisateurs (réservé administrateur).
/// </summary>
public class GestionUtilisateursViewModel : BaseViewModel
{
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IAuthService _authService;

    public GestionUtilisateursViewModel(
        IUtilisateurRepository utilisateurRepo,
        IAuthService authService)
    {
        _utilisateurRepo = utilisateurRepo;
        _authService = authService;

        ChargerCommand           = new AsyncRelayCommand(ChargerAsync);
        CreerCommand             = new RelayCommand(DemanderCreation);
        ActiverDesactiverCommand = new AsyncRelayCommand(ActiverDesactiverAsync,
            () => UtilisateurSelectionne != null);
        ReinitialiserMdpCommand  = new AsyncRelayCommand(ReinitialiserMdpAsync,
            () => UtilisateurSelectionne != null);

        _ = ChargerAsync();
    }

    // ── Propriétés ──────────────────────────────────────────────
    private ObservableCollection<Utilisateur> _utilisateurs = new();
    public ObservableCollection<Utilisateur> Utilisateurs
    {
        get => _utilisateurs;
        set => SetProperty(ref _utilisateurs, value);
    }

    private Utilisateur? _utilisateurSelectionne;
    public Utilisateur? UtilisateurSelectionne
    {
        get => _utilisateurSelectionne;
        set => SetProperty(ref _utilisateurSelectionne, value);
    }

    // ── Commandes ────────────────────────────────────────────────
    public AsyncRelayCommand ChargerCommand           { get; }
    public RelayCommand      CreerCommand             { get; }
    public AsyncRelayCommand ActiverDesactiverCommand { get; }
    public AsyncRelayCommand ReinitialiserMdpCommand  { get; }

    // Événement pour ouvrir le formulaire de création
    public event Action? DemanderCreation;

    // ── Actions ──────────────────────────────────────────────────
    private async Task ChargerAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var users = await _utilisateurRepo.GetAllAsync();
            Utilisateurs = new ObservableCollection<Utilisateur>(
                users.Where(u => !u.IsDeleted).OrderBy(u => u.Login));
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }

    private async Task ActiverDesactiverAsync()
    {
        if (UtilisateurSelectionne == null) return;
        ClearMessages();
        try
        {
            // Empêche de désactiver son propre compte
            if (UtilisateurSelectionne.Id == _authService.UtilisateurCourant?.Id)
            {
                ErrorMessage = "Vous ne pouvez pas désactiver votre propre compte.";
                return;
            }

            UtilisateurSelectionne.EstActif = !UtilisateurSelectionne.EstActif;
            await _utilisateurRepo.UpdateAsync(UtilisateurSelectionne);
            SuccessMessage = UtilisateurSelectionne.EstActif
                ? $"Compte \"{UtilisateurSelectionne.Login}\" activé."
                : $"Compte \"{UtilisateurSelectionne.Login}\" désactivé.";
            await ChargerAsync();
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
    }

    private async Task ReinitialiserMdpAsync()
    {
        if (UtilisateurSelectionne == null) return;
        ClearMessages();
        try
        {
            string nouveauMdp = $"Temp@{DateTime.Now.Year}!";
            UtilisateurSelectionne.MotDePasseHash = _authService.HacherMotDePasse(nouveauMdp);
            UtilisateurSelectionne.TentativesEchec = 0;
            UtilisateurSelectionne.BloquéJusquau = null;
            await _utilisateurRepo.UpdateAsync(UtilisateurSelectionne);
            SuccessMessage = $"Mot de passe réinitialisé à : {nouveauMdp}  (à changer à la prochaine connexion)";
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
    }

    public async Task AjouterUtilisateurAsync(
        string login, string nom, string prenom,
        string email, UserRole role, string motDePasse)
    {
        ClearMessages();
        if (await _utilisateurRepo.LoginExisteAsync(login))
        {
            ErrorMessage = $"Le login « {login} » est déjà utilisé.";
            return;
        }

        var user = new Utilisateur
        {
            Login          = login.Trim(),
            Nom            = nom.Trim(),
            Prenom         = prenom.Trim(),
            Email          = email.Trim(),
            Role           = role,
            EstActif       = true,
            MotDePasseHash = _authService.HacherMotDePasse(motDePasse),
            CreatedAt      = DateTime.UtcNow
        };

        await _utilisateurRepo.AddAsync(user);
        SuccessMessage = $"Utilisateur « {login} » créé avec succès.";
        await ChargerAsync();
    }
}
