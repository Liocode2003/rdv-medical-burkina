using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Implémentation du service métier Patient.
/// </summary>
public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepo;
    private readonly CarnetSanteDbContext _context;
    private readonly IAuditService _auditService;

    public PatientService(
        IPatientRepository patientRepo,
        CarnetSanteDbContext context,
        IAuditService auditService)
    {
        _patientRepo = patientRepo;
        _context = context;
        _auditService = auditService;
    }

    public async Task<Patient?> GetPatientCompletAsync(int patientId)
    {
        var patient = await _patientRepo.GetPatientCompletAsync(patientId);
        if (patient != null)
            await _auditService.EnregistrerAsync(TypeAction.ConsultationDossier,
                $"Consultation dossier patient #{patientId}",
                "Patient", patientId, patientId);
        return patient;
    }

    public async Task<IEnumerable<Patient>> RechercherPatientsAsync(
        string? nom = null, string? prenom = null,
        string? numeroCarnet = null, string? matricule = null)
        => await _patientRepo.RechercherAsync(nom, prenom, numeroCarnet, matricule);

    public async Task<Patient> CreerPatientAsync(Patient patient)
    {
        if (string.IsNullOrEmpty(patient.NumeroCarnet))
            patient.NumeroCarnet = await GenererNumeroCarnetAsync();

        var créé = await _patientRepo.AddAsync(patient);
        await _auditService.EnregistrerAsync(TypeAction.CreationPatient,
            $"Création patient {créé.NumeroCarnet}",
            "Patient", créé.Id, créé.Id);
        return créé;
    }

    public async Task<Patient> MettreAJourPatientAsync(Patient patient)
    {
        var mis = await _patientRepo.UpdateAsync(patient);
        await _auditService.EnregistrerAsync(TypeAction.ModificationPatient,
            $"Mise à jour patient #{patient.Id}",
            "Patient", patient.Id, patient.Id);
        return mis;
    }

    public async Task SupprimerPatientAsync(int patientId)
    {
        await _patientRepo.DeleteAsync(patientId);
        await _auditService.EnregistrerAsync(TypeAction.SuppressionPatient,
            $"Suppression patient #{patientId}",
            "Patient", patientId, patientId);
    }

    public async Task<string> GenererNumeroCarnetAsync()
    {
        var année = DateTime.Now.Year;
        var count = await _patientRepo.CountAsync() + 1;
        return $"CS-{année}-{count:D5}";
    }

    public async Task<Patient?> GetDernierPatientAsync()
        => await _context.Patients
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(p => !p.IsDeleted);

    // ── Ajout dans les modules ────────────────────────────────

    public async Task<Constante> AjouterConstanteAsync(Constante constante)
    {
        constante.IMC = constante.CalculerIMC();
        await _context.Constantes.AddAsync(constante);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            "Ajout constantes médicales", "Constante", constante.Id, constante.PatientId);
        return constante;
    }

    public async Task<OperationMedicale> AjouterOperationAsync(OperationMedicale operation)
    {
        await _context.OperationsMedicales.AddAsync(operation);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            $"Ajout opération: {operation.Diagnostic}", "OperationMedicale", operation.Id, operation.PatientId);
        return operation;
    }

    public async Task<Vaccination> AjouterVaccinationAsync(Vaccination vaccination)
    {
        await _context.Vaccinations.AddAsync(vaccination);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            $"Ajout vaccination: {vaccination.NomVaccin}", "Vaccination", vaccination.Id, vaccination.PatientId);
        return vaccination;
    }

    public async Task<VisiteSanitaire> AjouterVisiteAsync(VisiteSanitaire visite)
    {
        await _context.VisitesSanitaires.AddAsync(visite);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            "Ajout visite sanitaire", "VisiteSanitaire", visite.Id, visite.PatientId);
        return visite;
    }

    public async Task<Indisponibilite> AjouterIndisponibiliteAsync(Indisponibilite indisponibilite)
    {
        await _context.Indisponibilites.AddAsync(indisponibilite);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            $"Ajout indisponibilité: {indisponibilite.Motif}", "Indisponibilite", indisponibilite.Id, indisponibilite.PatientId);
        return indisponibilite;
    }

    public async Task<CertificatMedical> AjouterCertificatAsync(CertificatMedical certificat)
    {
        await _context.CertificatsMedicaux.AddAsync(certificat);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            $"Ajout certificat: {certificat.TypeCertificat}", "CertificatMedical", certificat.Id, certificat.PatientId);
        return certificat;
    }

    public async Task<DecisionReforme> AjouterDecisionReformeAsync(DecisionReforme decision)
    {
        await _context.DecisionsReforme.AddAsync(decision);
        await _context.SaveChangesAsync();
        await _auditService.EnregistrerAsync(TypeAction.AjoutDonnee,
            $"Ajout décision réforme: {decision.NumeroDecision}", "DecisionReforme", decision.Id, decision.PatientId);
        return decision;
    }
}
