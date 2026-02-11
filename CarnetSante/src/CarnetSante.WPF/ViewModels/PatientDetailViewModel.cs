using CarnetSante.Core.Enums;
using CarnetSante.Core.Interfaces;
using CarnetSante.Core.Models;
using CarnetSante.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarnetSante.WPF.ViewModels;

public partial class PatientDetailViewModel : BaseViewModel
{
    private readonly IPatientRepository _patientRepo;
    private readonly IEtatCivilRepository _etatCivilRepo;
    private readonly IContactUrgenceRepository _contactRepo;
    private readonly IConstanteRepository _constanteRepo;
    private readonly IExamenIncorporationRepository _examenRepo;
    private readonly IOperationMedicaleRepository _operationRepo;
    private readonly IVaccinationRepository _vaccinationRepo;
    private readonly IVisiteSanitaireRepository _visiteRepo;
    private readonly IIndisponibiliteRepository _indispoRepo;
    private readonly ICertificatMedicalRepository _certRepo;
    private readonly IDecisionReformeRepository _decisionRepo;
    private readonly IControleFinServiceRepository _controleRepo;
    private readonly IPdfService _pdfService;
    private readonly INumeroCarnetService _numService;

    // ─── Patient Principal ───
    [ObservableProperty] private Patient? _patient;
    [ObservableProperty] private bool _isNewPatient;
    [ObservableProperty] private string _pageTitle = "Nouveau Patient";

    // A - Identité
    [ObservableProperty] private string _nom = string.Empty;
    [ObservableProperty] private string _prenoms = string.Empty;
    [ObservableProperty] private DateTime _dateNaissance = DateTime.Now.AddYears(-30);
    [ObservableProperty] private string _lieuNaissance = string.Empty;
    [ObservableProperty] private Sexe _sexe = Sexe.Masculin;
    [ObservableProperty] private GroupeSanguin _groupeSanguin = GroupeSanguin.Inconnu;
    [ObservableProperty] private string _matricule = string.Empty;
    [ObservableProperty] private string _numeroCarnet = string.Empty;
    [ObservableProperty] private string _telephone = string.Empty;
    [ObservableProperty] private string _adresse = string.Empty;

    // A - État Civil
    [ObservableProperty] private string _situationMatrimoniale = string.Empty;
    [ObservableProperty] private string _nomPere = string.Empty;
    [ObservableProperty] private string _nomMere = string.Empty;
    [ObservableProperty] private int _nombreEnfants;
    [ObservableProperty] private string _profession = string.Empty;
    [ObservableProperty] private string _corps = string.Empty;
    [ObservableProperty] private string _grade = string.Empty;
    [ObservableProperty] private string _affectation = string.Empty;
    [ObservableProperty] private string _serviceOrigine = string.Empty;
    [ObservableProperty] private DateTime? _dateIntegration;
    [ObservableProperty] private string _observationsEtatCivil = string.Empty;

    // B - Constantes
    [ObservableProperty] private ObservableCollection<Constante> _constantes = new();
    [ObservableProperty] private Constante? _selectedConstante;
    [ObservableProperty] private Constante _editingConstante = new();

    // C - Examen Incorporation
    [ObservableProperty] private DateTime _dateExamenIncorporation = DateTime.Now;
    [ObservableProperty] private Aptitude _aptitudeIncorporation = Aptitude.Apte;
    [ObservableProperty] private string _medecinExaminateur = string.Empty;
    [ObservableProperty] private string _antecedentsMedicaux = string.Empty;
    [ObservableProperty] private string _antecedentsChirurgicaux = string.Empty;
    [ObservableProperty] private string _antecedentsFamiliaux = string.Empty;
    [ObservableProperty] private string _examenClinique = string.Empty;
    [ObservableProperty] private string _examensComplementaires = string.Empty;
    [ObservableProperty] private string _conclusionExamen = string.Empty;

    // D - Opérations
    [ObservableProperty] private ObservableCollection<OperationMedicale> _operations = new();
    [ObservableProperty] private OperationMedicale? _selectedOperation;
    [ObservableProperty] private OperationMedicale _editingOperation = new();

    // E - Vaccinations
    [ObservableProperty] private ObservableCollection<Vaccination> _vaccinations = new();
    [ObservableProperty] private Vaccination? _selectedVaccination;
    [ObservableProperty] private Vaccination _editingVaccination = new();

    // F - Visites
    [ObservableProperty] private ObservableCollection<VisiteSanitaire> _visites = new();
    [ObservableProperty] private VisiteSanitaire? _selectedVisite;
    [ObservableProperty] private VisiteSanitaire _editingVisite = new();

    // G - Indisponibilités
    [ObservableProperty] private ObservableCollection<Indisponibilite> _indisponibilites = new();
    [ObservableProperty] private Indisponibilite? _selectedIndispo;
    [ObservableProperty] private Indisponibilite _editingIndispo = new();

    // H - Certificats
    [ObservableProperty] private ObservableCollection<CertificatMedical> _certificats = new();
    [ObservableProperty] private CertificatMedical? _selectedCertificat;
    [ObservableProperty] private CertificatMedical _editingCertificat = new();

    // I - Décisions Réforme
    [ObservableProperty] private ObservableCollection<DecisionReformeMed> _decisions = new();
    [ObservableProperty] private DecisionReformeMed? _selectedDecision;
    [ObservableProperty] private DecisionReformeMed _editingDecision = new();

    // J - Contrôle Fin Service
    [ObservableProperty] private DateTime _dateControle = DateTime.Now;
    [ObservableProperty] private string _medecinControleur = string.Empty;
    [ObservableProperty] private Aptitude _aptitudeFinal = Aptitude.Apte;
    [ObservableProperty] private string _bilanSante = string.Empty;
    [ObservableProperty] private string _pathologiesChroniques = string.Empty;
    [ObservableProperty] private string _traitementsEnCours = string.Empty;
    [ObservableProperty] private string _recommandationsSante = string.Empty;
    [ObservableProperty] private string _conclusionControle = string.Empty;
    [ObservableProperty] private string _observationsControle = string.Empty;

    // Enums for ComboBoxes
    public IEnumerable<Sexe> ListeSexe => Enum.GetValues<Sexe>();
    public IEnumerable<GroupeSanguin> ListeGroupeSanguin => Enum.GetValues<GroupeSanguin>();
    public IEnumerable<Aptitude> ListeAptitude => Enum.GetValues<Aptitude>();
    public IEnumerable<TypeVaccin> ListeTypeVaccin => Enum.GetValues<TypeVaccin>();
    public IEnumerable<TypeIntervention> ListeTypeIntervention => Enum.GetValues<TypeIntervention>();
    public IEnumerable<DecisionReforme> ListeDecisionReforme => Enum.GetValues<DecisionReforme>();

    public event Action? SaveCompleted;
    public event Action? NavigateBackRequested;

    public PatientDetailViewModel(
        IPatientRepository patientRepo,
        IEtatCivilRepository etatCivilRepo,
        IContactUrgenceRepository contactRepo,
        IConstanteRepository constanteRepo,
        IExamenIncorporationRepository examenRepo,
        IOperationMedicaleRepository operationRepo,
        IVaccinationRepository vaccinationRepo,
        IVisiteSanitaireRepository visiteRepo,
        IIndisponibiliteRepository indispoRepo,
        ICertificatMedicalRepository certRepo,
        IDecisionReformeRepository decisionRepo,
        IControleFinServiceRepository controleRepo,
        IPdfService pdfService,
        INumeroCarnetService numService)
    {
        _patientRepo = patientRepo;
        _etatCivilRepo = etatCivilRepo;
        _contactRepo = contactRepo;
        _constanteRepo = constanteRepo;
        _examenRepo = examenRepo;
        _operationRepo = operationRepo;
        _vaccinationRepo = vaccinationRepo;
        _visiteRepo = visiteRepo;
        _indispoRepo = indispoRepo;
        _certRepo = certRepo;
        _decisionRepo = decisionRepo;
        _controleRepo = controleRepo;
        _pdfService = pdfService;
        _numService = numService;
    }

    [RelayCommand]
    public async Task LoadPatientAsync(int patientId)
    {
        try
        {
            SetBusy("Chargement du dossier...");
            var p = await _patientRepo.GetByIdAsync(patientId);
            if (p == null) { SetError("Patient introuvable."); return; }

            Patient = p;
            IsNewPatient = false;
            PageTitle = p.NomComplet;

            // Identité
            Nom = p.Nom; Prenoms = p.Prenoms; DateNaissance = p.DateNaissance;
            LieuNaissance = p.LieuNaissance; Sexe = p.Sexe; GroupeSanguin = p.GroupeSanguin;
            Matricule = p.Matricule; NumeroCarnet = p.NumeroCarnet;
            Telephone = p.Telephone; Adresse = p.Adresse;

            // A - État Civil
            var ec = await _etatCivilRepo.GetByPatientAsync(patientId);
            if (ec != null)
            {
                SituationMatrimoniale = ec.SituationMatrimoniale; NomPere = ec.NomPere;
                NomMere = ec.NomMere; NombreEnfants = ec.NombreEnfants;
                Profession = ec.Profession; Corps = ec.Corps; Grade = ec.Grade;
                Affectation = ec.Affectation; ServiceOrigine = ec.ServiceOrigine;
                DateIntegration = ec.DateIntegration; ObservationsEtatCivil = ec.Observations;
            }

            // B - Constantes
            var cst = await _constanteRepo.GetByPatientAsync(patientId);
            Constantes = new ObservableCollection<Constante>(cst);

            // C - Examen Incorporation
            var exam = await _examenRepo.GetByPatientAsync(patientId);
            if (exam != null)
            {
                DateExamenIncorporation = exam.DateExamen; AptitudeIncorporation = exam.Aptitude;
                MedecinExaminateur = exam.MedecinExaminateur; AntecedentsMedicaux = exam.AntecedentsMedicaux;
                AntecedentsChirurgicaux = exam.AntecedentsChirurgicaux; AntecedentsFamiliaux = exam.AntecedentsFamiliaux;
                ExamenClinique = exam.ExamenClinique; ExamensComplementaires = exam.ExamensComplementaires;
                ConclusionExamen = exam.Conclusion;
            }

            // D-I
            var ops = await _operationRepo.GetByPatientAsync(patientId);
            Operations = new ObservableCollection<OperationMedicale>(ops);

            var vaccs = await _vaccinationRepo.GetByPatientAsync(patientId);
            Vaccinations = new ObservableCollection<Vaccination>(vaccs);

            var vis = await _visiteRepo.GetByPatientAsync(patientId);
            Visites = new ObservableCollection<VisiteSanitaire>(vis);

            var ind = await _indispoRepo.GetByPatientAsync(patientId);
            Indisponibilites = new ObservableCollection<Indisponibilite>(ind);

            var cert = await _certRepo.GetByPatientAsync(patientId);
            Certificats = new ObservableCollection<CertificatMedical>(cert);

            var dec = await _decisionRepo.GetByPatientAsync(patientId);
            Decisions = new ObservableCollection<DecisionReformeMed>(dec);

            // J - Contrôle fin service
            var ctrl = await _controleRepo.GetByPatientAsync(patientId);
            if (ctrl != null)
            {
                DateControle = ctrl.DateControle; MedecinControleur = ctrl.MedecinControleur;
                AptitudeFinal = ctrl.AptitudeFinal; BilanSante = ctrl.BilanSante;
                PathologiesChroniques = ctrl.PathologiesChroniques; TraitementsEnCours = ctrl.TraitementsEnCours;
                RecommandationsSante = ctrl.RecommandationsSante; ConclusionControle = ctrl.Conclusion;
                ObservationsControle = ctrl.Observations;
            }

            SetIdle();
        }
        catch (Exception ex)
        {
            SetError($"Erreur de chargement: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task InitNewPatientAsync()
    {
        IsNewPatient = true;
        PageTitle = "Nouveau Patient";
        NumeroCarnet = await _numService.GenerateAsync();
        SetIdle();
    }

    [RelayCommand]
    public async Task SavePatientAsync()
    {
        if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenoms))
        {
            SetError("Le nom et les prénoms sont obligatoires.");
            return;
        }

        try
        {
            SetBusy("Enregistrement en cours...");

            if (IsNewPatient)
            {
                var p = new Patient
                {
                    Nom = Nom, Prenoms = Prenoms, DateNaissance = DateNaissance,
                    LieuNaissance = LieuNaissance, Sexe = Sexe, GroupeSanguin = GroupeSanguin,
                    Matricule = Matricule, NumeroCarnet = NumeroCarnet,
                    Telephone = Telephone, Adresse = Adresse
                };
                Patient = await _patientRepo.CreateAsync(p);
                IsNewPatient = false;
                PageTitle = Patient.NomComplet;
            }
            else if (Patient != null)
            {
                Patient.Nom = Nom; Patient.Prenoms = Prenoms; Patient.DateNaissance = DateNaissance;
                Patient.LieuNaissance = LieuNaissance; Patient.Sexe = Sexe; Patient.GroupeSanguin = GroupeSanguin;
                Patient.Matricule = Matricule; Patient.Telephone = Telephone; Patient.Adresse = Adresse;
                await _patientRepo.UpdateAsync(Patient);
                PageTitle = Patient.NomComplet;
            }

            if (Patient != null)
            {
                // Save État Civil
                var ec = await _etatCivilRepo.GetByPatientAsync(Patient.Id) ?? new EtatCivil { PatientId = Patient.Id };
                ec.SituationMatrimoniale = SituationMatrimoniale; ec.NomPere = NomPere; ec.NomMere = NomMere;
                ec.NombreEnfants = NombreEnfants; ec.Profession = Profession; ec.Corps = Corps;
                ec.Grade = Grade; ec.Affectation = Affectation; ec.ServiceOrigine = ServiceOrigine;
                ec.DateIntegration = DateIntegration; ec.Observations = ObservationsEtatCivil;
                await _etatCivilRepo.SaveAsync(ec);

                // Save Examen incorporation
                var exam = await _examenRepo.GetByPatientAsync(Patient.Id) ?? new ExamenIncorporation { PatientId = Patient.Id };
                exam.DateExamen = DateExamenIncorporation; exam.Aptitude = AptitudeIncorporation;
                exam.MedecinExaminateur = MedecinExaminateur; exam.AntecedentsMedicaux = AntecedentsMedicaux;
                exam.AntecedentsChirurgicaux = AntecedentsChirurgicaux; exam.AntecedentsFamiliaux = AntecedentsFamiliaux;
                exam.ExamenClinique = ExamenClinique; exam.ExamensComplementaires = ExamensComplementaires;
                exam.Conclusion = ConclusionExamen;
                await _examenRepo.SaveAsync(exam);

                // Save Contrôle fin service
                var ctrl = await _controleRepo.GetByPatientAsync(Patient.Id) ?? new ControleFinService { PatientId = Patient.Id };
                ctrl.DateControle = DateControle; ctrl.MedecinControleur = MedecinControleur;
                ctrl.AptitudeFinal = AptitudeFinal; ctrl.BilanSante = BilanSante;
                ctrl.PathologiesChroniques = PathologiesChroniques; ctrl.TraitementsEnCours = TraitementsEnCours;
                ctrl.RecommandationsSante = RecommandationsSante; ctrl.Conclusion = ConclusionControle;
                ctrl.Observations = ObservationsControle;
                await _controleRepo.SaveAsync(ctrl);
            }

            SetIdle("Dossier enregistré avec succès");
            SaveCompleted?.Invoke();
        }
        catch (Exception ex)
        {
            SetError($"Erreur d'enregistrement: {ex.Message}");
        }
    }

    // ── Constantes CRUD ──
    [RelayCommand]
    private void NewConstante()
    {
        EditingConstante = new Constante { PatientId = Patient?.Id ?? 0, DateMesure = DateTime.Now };
    }

    [RelayCommand]
    private async Task SaveConstanteAsync()
    {
        if (Patient == null) return;
        try
        {
            EditingConstante.PatientId = Patient.Id;
            if (EditingConstante.Poids.HasValue && EditingConstante.Taille.HasValue && EditingConstante.Taille > 0)
            {
                var tailleM = EditingConstante.Taille.Value / 100m;
                EditingConstante.IMC = Math.Round(EditingConstante.Poids.Value / (tailleM * tailleM), 1);
            }
            var saved = await _constanteRepo.SaveAsync(EditingConstante);
            if (!Constantes.Any(c => c.Id == saved.Id))
                Constantes.Insert(0, saved);
            else
            {
                var idx = Constantes.IndexOf(Constantes.First(c => c.Id == saved.Id));
                Constantes[idx] = saved;
            }
            EditingConstante = new Constante { PatientId = Patient.Id, DateMesure = DateTime.Now };
        }
        catch (Exception ex) { SetError(ex.Message); }
    }

    [RelayCommand]
    private async Task DeleteConstanteAsync()
    {
        if (SelectedConstante == null) return;
        await _constanteRepo.DeleteAsync(SelectedConstante.Id);
        Constantes.Remove(SelectedConstante);
    }

    // ── Operations CRUD ──
    [RelayCommand] private void NewOperation() =>
        EditingOperation = new OperationMedicale { PatientId = Patient?.Id ?? 0, DateOperation = DateTime.Now };

    [RelayCommand]
    private async Task SaveOperationAsync()
    {
        if (Patient == null) return;
        EditingOperation.PatientId = Patient.Id;
        var saved = await _operationRepo.SaveAsync(EditingOperation);
        if (!Operations.Any(o => o.Id == saved.Id)) Operations.Insert(0, saved);
        else { var idx = Operations.IndexOf(Operations.First(o => o.Id == saved.Id)); Operations[idx] = saved; }
        EditingOperation = new OperationMedicale { PatientId = Patient.Id, DateOperation = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteOperationAsync()
    {
        if (SelectedOperation == null) return;
        await _operationRepo.DeleteAsync(SelectedOperation.Id);
        Operations.Remove(SelectedOperation);
    }

    // ── Vaccinations CRUD ──
    [RelayCommand] private void NewVaccination() =>
        EditingVaccination = new Vaccination { PatientId = Patient?.Id ?? 0, DateVaccination = DateTime.Now };

    [RelayCommand]
    private async Task SaveVaccinationAsync()
    {
        if (Patient == null) return;
        EditingVaccination.PatientId = Patient.Id;
        var saved = await _vaccinationRepo.SaveAsync(EditingVaccination);
        if (!Vaccinations.Any(v => v.Id == saved.Id)) Vaccinations.Insert(0, saved);
        else { var idx = Vaccinations.IndexOf(Vaccinations.First(v => v.Id == saved.Id)); Vaccinations[idx] = saved; }
        EditingVaccination = new Vaccination { PatientId = Patient.Id, DateVaccination = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteVaccinationAsync()
    {
        if (SelectedVaccination == null) return;
        await _vaccinationRepo.DeleteAsync(SelectedVaccination.Id);
        Vaccinations.Remove(SelectedVaccination);
    }

    // ── Visites CRUD ──
    [RelayCommand] private void NewVisite() =>
        EditingVisite = new VisiteSanitaire { PatientId = Patient?.Id ?? 0, DateVisite = DateTime.Now };

    [RelayCommand]
    private async Task SaveVisiteAsync()
    {
        if (Patient == null) return;
        EditingVisite.PatientId = Patient.Id;
        var saved = await _visiteRepo.SaveAsync(EditingVisite);
        if (!Visites.Any(v => v.Id == saved.Id)) Visites.Insert(0, saved);
        else { var idx = Visites.IndexOf(Visites.First(v => v.Id == saved.Id)); Visites[idx] = saved; }
        EditingVisite = new VisiteSanitaire { PatientId = Patient.Id, DateVisite = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteVisiteAsync()
    {
        if (SelectedVisite == null) return;
        await _visiteRepo.DeleteAsync(SelectedVisite.Id);
        Visites.Remove(SelectedVisite);
    }

    // ── Indisponibilités CRUD ──
    [RelayCommand] private void NewIndispo() =>
        EditingIndispo = new Indisponibilite { PatientId = Patient?.Id ?? 0, DateDebut = DateTime.Now };

    [RelayCommand]
    private async Task SaveIndispoAsync()
    {
        if (Patient == null) return;
        EditingIndispo.PatientId = Patient.Id;
        var saved = await _indispoRepo.SaveAsync(EditingIndispo);
        if (!Indisponibilites.Any(i => i.Id == saved.Id)) Indisponibilites.Insert(0, saved);
        else { var idx = Indisponibilites.IndexOf(Indisponibilites.First(i => i.Id == saved.Id)); Indisponibilites[idx] = saved; }
        EditingIndispo = new Indisponibilite { PatientId = Patient.Id, DateDebut = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteIndispoAsync()
    {
        if (SelectedIndispo == null) return;
        await _indispoRepo.DeleteAsync(SelectedIndispo.Id);
        Indisponibilites.Remove(SelectedIndispo);
    }

    // ── Certificats CRUD ──
    [RelayCommand] private void NewCertificat() =>
        EditingCertificat = new CertificatMedical { PatientId = Patient?.Id ?? 0, DateCertificat = DateTime.Now };

    [RelayCommand]
    private async Task SaveCertificatAsync()
    {
        if (Patient == null) return;
        EditingCertificat.PatientId = Patient.Id;
        var saved = await _certRepo.SaveAsync(EditingCertificat);
        if (!Certificats.Any(c => c.Id == saved.Id)) Certificats.Insert(0, saved);
        else { var idx = Certificats.IndexOf(Certificats.First(c => c.Id == saved.Id)); Certificats[idx] = saved; }
        EditingCertificat = new CertificatMedical { PatientId = Patient.Id, DateCertificat = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteCertificatAsync()
    {
        if (SelectedCertificat == null) return;
        await _certRepo.DeleteAsync(SelectedCertificat.Id);
        Certificats.Remove(SelectedCertificat);
    }

    // ── Décisions CRUD ──
    [RelayCommand] private void NewDecision() =>
        EditingDecision = new DecisionReformeMed { PatientId = Patient?.Id ?? 0, DateDecision = DateTime.Now };

    [RelayCommand]
    private async Task SaveDecisionAsync()
    {
        if (Patient == null) return;
        EditingDecision.PatientId = Patient.Id;
        var saved = await _decisionRepo.SaveAsync(EditingDecision);
        if (!Decisions.Any(d => d.Id == saved.Id)) Decisions.Insert(0, saved);
        else { var idx = Decisions.IndexOf(Decisions.First(d => d.Id == saved.Id)); Decisions[idx] = saved; }
        EditingDecision = new DecisionReformeMed { PatientId = Patient.Id, DateDecision = DateTime.Now };
    }

    [RelayCommand]
    private async Task DeleteDecisionAsync()
    {
        if (SelectedDecision == null) return;
        await _decisionRepo.DeleteAsync(SelectedDecision.Id);
        Decisions.Remove(SelectedDecision);
    }

    // ── PDF ──
    [RelayCommand]
    private async Task ExportPdfAsync()
    {
        if (Patient == null) return;
        try
        {
            SetBusy("Génération du PDF...");
            var p = await _patientRepo.GetByIdAsync(Patient.Id);
            if (p == null) return;
            var path = await _pdfService.ExportCarnetCompletAsync(p);
            SetIdle($"PDF exporté : {path}");
        }
        catch (Exception ex) { SetError($"Erreur PDF: {ex.Message}"); }
    }

    [RelayCommand]
    private void Back() => NavigateBackRequested?.Invoke();
}
