using CarnetSante.Core.Models;
using CarnetSante.Core.Services;

namespace CarnetSante.WPF.ViewModels.Patient;

/// <summary>
/// ViewModel de la fiche patient complète (onglets A→J).
/// </summary>
public class PatientViewModel : BaseViewModel
{
    private readonly IPatientService _patientService;
    private readonly IPdfService _pdfService;
    private readonly IAuthService _authService;

    public PatientViewModel(
        IPatientService patientService,
        IPdfService pdfService,
        IAuthService authService)
    {
        _patientService = patientService;
        _pdfService = pdfService;
        _authService = authService;

        SauvegarderCommand = new AsyncRelayCommand(SauvegarderAsync, () => EstModifie && EstMedecin);
        ImprimerCarnetCommand = new AsyncRelayCommand(ImprimerCarnetAsync);
        ImprimerSectionCommand = new AsyncRelayCommand<string>(ImprimerSectionAsync);
        NouvelleConstanteCommand = new RelayCommand(NouvelleConstante, () => EstMedecin);
        NouvelleVaccinationCommand = new RelayCommand(NouvelleVaccination, () => EstMedecin);
        NouvelleVisiteCommand = new RelayCommand(NouvelleVisite, () => EstMedecin);
        NouvelleIndisponibiliteCommand = new RelayCommand(NouvelleIndisponibilite, () => EstMedecin);
        NouveauCertificatCommand = new RelayCommand(NouveauCertificat, () => EstMedecin);
        NouvelleDecisionReformeCommand = new RelayCommand(NouvelleDecisionReforme, () => EstAdmin);
    }

    // ── Patient chargé ────────────────────────────────────────
    private Core.Models.Patient? _patient;
    public Core.Models.Patient? Patient
    {
        get => _patient;
        set
        {
            SetProperty(ref _patient, value);
            OnPropertyChanged(nameof(TitrePatient));
            OnPropertyChanged(nameof(EtatCivil));
        }
    }

    public EtatCivil? EtatCivil => _patient?.EtatCivil;

    public string TitrePatient => _patient?.EtatCivil != null
        ? $"{_patient.EtatCivil.Nom.ToUpper()} {_patient.EtatCivil.Prenoms} - {_patient.NumeroCarnet}"
        : "Nouveau Patient";

    private bool _estModifie;
    public bool EstModifie
    {
        get => _estModifie;
        set => SetProperty(ref _estModifie, value);
    }

    // Onglet actif (navigation A→J)
    private int _ongletActif;
    public int OngletActif
    {
        get => _ongletActif;
        set => SetProperty(ref _ongletActif, value);
    }

    // ── Permissions ───────────────────────────────────────────
    public bool EstMedecin => _authService.ARole(
        Core.Enums.UserRole.Administrateur, Core.Enums.UserRole.Medecin);
    public bool EstAdmin => _authService.ARole(Core.Enums.UserRole.Administrateur);
    public bool EstLectureSeule => !EstMedecin;

    // ── Commandes ─────────────────────────────────────────────
    public AsyncRelayCommand SauvegarderCommand { get; }
    public AsyncRelayCommand ImprimerCarnetCommand { get; }
    public AsyncRelayCommand<string> ImprimerSectionCommand { get; }
    public RelayCommand NouvelleConstanteCommand { get; }
    public RelayCommand NouvelleVaccinationCommand { get; }
    public RelayCommand NouvelleVisiteCommand { get; }
    public RelayCommand NouvelleIndisponibiliteCommand { get; }
    public RelayCommand NouveauCertificatCommand { get; }
    public RelayCommand NouvelleDecisionReformeCommand { get; }

    public event Action? DemanderNouvelleConstante;
    public event Action? DemanderNouvelleVaccination;
    public event Action? DemanderNouvelleVisite;
    public event Action? DemanderNouvelleIndisponibilite;
    public event Action? DemanderNouveauCertificat;
    public event Action? DemanderNouvelleDecisionReforme;

    // ── Chargement ────────────────────────────────────────────
    public async Task ChargerPatientAsync(int patientId)
    {
        IsLoading = true;
        try
        {
            Patient = await _patientService.GetPatientCompletAsync(patientId);
            EstModifie = false;
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }

    public async Task InitialiserNouveauPatientAsync()
    {
        var numero = await _patientService.GenererNumeroCarnetAsync();
        Patient = new Core.Models.Patient
        {
            NumeroCarnet = numero,
            EtatCivil = new EtatCivil { DateNaissance = DateTime.Today.AddYears(-25) }
        };
        EstModifie = true;
    }

    // ── Sauvegarde ────────────────────────────────────────────
    private async Task SauvegarderAsync()
    {
        if (Patient == null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            if (Patient.Id == 0)
                Patient = await _patientService.CreerPatientAsync(Patient);
            else
                await _patientService.MettreAJourPatientAsync(Patient);

            EstModifie = false;
            SuccessMessage = "Données sauvegardées avec succès.";
        }
        catch (Exception ex) { ErrorMessage = $"Erreur lors de la sauvegarde : {ex.Message}"; }
        finally { IsLoading = false; }
    }

    // ── Impression PDF ────────────────────────────────────────
    private async Task ImprimerCarnetAsync()
    {
        if (Patient == null) return;
        IsLoading = true;
        try
        {
            var pdf = await _pdfService.GenererCarnetCompletAsync(Patient);
            await _pdfService.OuvrirPdfAsync(pdf, $"Carnet_{Patient.NumeroCarnet}.pdf");
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }

    private async Task ImprimerSectionAsync(string? section)
    {
        if (Patient == null || section == null) return;
        try
        {
            var pdf = await _pdfService.GenererSectionAsync(Patient, section);
            await _pdfService.OuvrirPdfAsync(pdf, $"Section_{section}_{Patient.NumeroCarnet}.pdf");
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
    }

    // ── Ajouts dans les modules ───────────────────────────────
    private void NouvelleConstante() => DemanderNouvelleConstante?.Invoke();
    private void NouvelleVaccination() => DemanderNouvelleVaccination?.Invoke();
    private void NouvelleVisite() => DemanderNouvelleVisite?.Invoke();
    private void NouvelleIndisponibilite() => DemanderNouvelleIndisponibilite?.Invoke();
    private void NouveauCertificat() => DemanderNouveauCertificat?.Invoke();
    private void NouvelleDecisionReforme() => DemanderNouvelleDecisionReforme?.Invoke();

    public void MarquerModifie() => EstModifie = true;
}

/// <summary>Commande générique typée.</summary>
public class AsyncRelayCommand<T> : System.Windows.Input.ICommand
{
    private readonly Func<T?, Task> _execute;
    private readonly Func<T?, bool>? _canExecute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => System.Windows.Input.CommandManager.RequerySuggested += value;
        remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) =>
        !_isExecuting && (_canExecute?.Invoke((T?)parameter) ?? true);

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        _isExecuting = true;
        System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        try { await _execute((T?)parameter); }
        finally
        {
            _isExecuting = false;
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }
    }
}
