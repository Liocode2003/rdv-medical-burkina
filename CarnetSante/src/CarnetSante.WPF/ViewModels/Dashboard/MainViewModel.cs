using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using System.Collections.ObjectModel;

namespace CarnetSante.WPF.ViewModels.Dashboard;

/// <summary>
/// ViewModel principal du tableau de bord.
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly IPatientService _patientService;
    private readonly IAuthService _authService;
    private readonly IAuditService _auditService;

    public MainViewModel(
        IPatientService patientService,
        IAuthService authService,
        IAuditService auditService)
    {
        _patientService = patientService;
        _authService = authService;
        _auditService = auditService;

        ChargerPatientsCommand     = new AsyncRelayCommand(ChargerPatientsAsync);
        RechercherCommand          = new AsyncRelayCommand(RechercherAsync);
        NouveauPatientCommand      = new RelayCommand(NouveauPatient);
        DeconnecterCommand         = new AsyncRelayCommand(DeconnecterAsync);
        OuvrirUtilisateursCommand  = new RelayCommand(() => DemanderOuvrirUtilisateurs?.Invoke(), () => EstAdmin);
        OuvrirJournalAuditCommand  = new RelayCommand(() => DemanderOuvrirJournalAudit?.Invoke(), () => EstAdmin);
        OuvrirChangerMdpCommand    = new RelayCommand(() => DemanderChangerMdp?.Invoke());
        SauvegarderBDDCommand      = new AsyncRelayCommand(SauvegarderBDDAsync, () => EstAdmin);

        _ = ChargerPatientsAsync();
    }

    // ── Propriétés ──────────────────────────────────────────
    private ObservableCollection<Patient> _patients = new();
    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set => SetProperty(ref _patients, value);
    }

    private Patient? _patientSelectionne;
    public Patient? PatientSelectionne
    {
        get => _patientSelectionne;
        set => SetProperty(ref _patientSelectionne, value);
    }

    private string _searchQuery = string.Empty;
    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    private int _totalPatients;
    public int TotalPatients
    {
        get => _totalPatients;
        set => SetProperty(ref _totalPatients, value);
    }

    // Info utilisateur connecté
    public string NomUtilisateur =>
        _authService.UtilisateurCourant != null
            ? $"{_authService.UtilisateurCourant.Prenom} {_authService.UtilisateurCourant.Nom}"
            : "Inconnu";

    public string RoleUtilisateur =>
        _authService.UtilisateurCourant?.Role.ToString() ?? "";

    public bool EstAdmin => _authService.ARole(Core.Enums.UserRole.Administrateur);
    public bool EstMedecin => _authService.ARole(
        Core.Enums.UserRole.Administrateur, Core.Enums.UserRole.Medecin);

    // ── Commandes ────────────────────────────────────────────
    public AsyncRelayCommand ChargerPatientsCommand    { get; }
    public AsyncRelayCommand RechercherCommand         { get; }
    public RelayCommand      NouveauPatientCommand     { get; }
    public AsyncRelayCommand DeconnecterCommand        { get; }
    public RelayCommand      OuvrirUtilisateursCommand { get; }
    public RelayCommand      OuvrirJournalAuditCommand { get; }
    public RelayCommand      OuvrirChangerMdpCommand   { get; }
    public AsyncRelayCommand SauvegarderBDDCommand     { get; }

    // Événements de navigation
    public event Action<Patient?>? OuvrirFichePatient;
    public event Action? DemanderDeconnexion;
    public event Action? DemanderOuvrirUtilisateurs;
    public event Action? DemanderOuvrirJournalAudit;
    public event Action? DemanderChangerMdp;

    // ── Actions ──────────────────────────────────────────────
    private async Task ChargerPatientsAsync()
    {
        IsLoading = true;
        try
        {
            var patients = await _patientService.RechercherPatientsAsync();
            Patients = new ObservableCollection<Patient>(patients);
            TotalPatients = Patients.Count;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur lors du chargement : {ex.Message}";
        }
        finally { IsLoading = false; }
    }

    private async Task RechercherAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await ChargerPatientsAsync();
            return;
        }

        IsLoading = true;
        try
        {
            var résultats = await _patientService.RechercherPatientsAsync(
                nom: SearchQuery, prenom: SearchQuery, numeroCarnet: SearchQuery);
            Patients = new ObservableCollection<Patient>(résultats);
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }

    private void NouveauPatient() => OuvrirFichePatient?.Invoke(null);

    private async Task DeconnecterAsync()
    {
        await _auditService.EnregistrerAsync(Core.Enums.TypeAction.Deconnexion, "Déconnexion utilisateur");
        await _authService.DeconnecterAsync();
        DemanderDeconnexion?.Invoke();
    }

    private async Task SauvegarderBDDAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            // Sauvegarde la base SQLite vers un fichier horodaté
            string srcPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CarnetSante", "carnet_sante.db");

            string backupDir = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CarnetSante", "Backups");

            System.IO.Directory.CreateDirectory(backupDir);

            string backupFile = System.IO.Path.Combine(backupDir,
                $"carnet_sante_{DateTime.Now:yyyyMMdd_HHmmss}.db");

            await Task.Run(() => System.IO.File.Copy(srcPath, backupFile, overwrite: false));

            await _auditService.EnregistrerAsync(
                Core.Enums.TypeAction.Sauvegarde,
                $"Sauvegarde manuelle : {backupFile}");

            SuccessMessage = $"Sauvegarde créée : {System.IO.Path.GetFileName(backupFile)}";
        }
        catch (Exception ex) { ErrorMessage = $"Erreur sauvegarde : {ex.Message}"; }
        finally { IsLoading = false; }
    }

    public void OuvrirPatient(Patient patient) => OuvrirFichePatient?.Invoke(patient);
}
