using CarnetSante.Core.Models;

namespace CarnetSante.Core.Services;

/// <summary>
/// Interface du service métier Patient.
/// </summary>
public interface IPatientService
{
    Task<Patient?> GetPatientCompletAsync(int patientId);
    Task<IEnumerable<Patient>> RechercherPatientsAsync(string? nom = null, string? prenom = null,
        string? numeroCarnet = null, string? matricule = null);
    Task<Patient> CreerPatientAsync(Patient patient);
    Task<Patient> MettreAJourPatientAsync(Patient patient);
    Task SupprimerPatientAsync(int patientId);
    Task<string> GenererNumeroCarnetAsync();
    Task<Patient?> GetDernierPatientAsync();

    // Modules spécifiques
    Task<Constante> AjouterConstanteAsync(Constante constante);
    Task<OperationMedicale> AjouterOperationAsync(OperationMedicale operation);
    Task<Vaccination> AjouterVaccinationAsync(Vaccination vaccination);
    Task<VisiteSanitaire> AjouterVisiteAsync(VisiteSanitaire visite);
    Task<Indisponibilite> AjouterIndisponibiliteAsync(Indisponibilite indisponibilite);
    Task<CertificatMedical> AjouterCertificatAsync(CertificatMedical certificat);
    Task<DecisionReforme> AjouterDecisionReformeAsync(DecisionReforme decision);
}
