using CarnetSante.Core.Models;

namespace CarnetSante.Core.Interfaces;

public interface IConstanteRepository
{
    Task<List<Constante>> GetByPatientAsync(int patientId);
    Task<Constante> SaveAsync(Constante constante);
    Task DeleteAsync(int id);
}

public interface IEtatCivilRepository
{
    Task<EtatCivil?> GetByPatientAsync(int patientId);
    Task<EtatCivil> SaveAsync(EtatCivil etatCivil);
}

public interface IContactUrgenceRepository
{
    Task<List<ContactUrgence>> GetByPatientAsync(int patientId);
    Task<ContactUrgence> SaveAsync(ContactUrgence contact);
    Task DeleteAsync(int id);
}

public interface IExamenIncorporationRepository
{
    Task<ExamenIncorporation?> GetByPatientAsync(int patientId);
    Task<ExamenIncorporation> SaveAsync(ExamenIncorporation examen);
}

public interface IOperationMedicaleRepository
{
    Task<List<OperationMedicale>> GetByPatientAsync(int patientId);
    Task<OperationMedicale> SaveAsync(OperationMedicale operation);
    Task DeleteAsync(int id);
}

public interface IVaccinationRepository
{
    Task<List<Vaccination>> GetByPatientAsync(int patientId);
    Task<Vaccination> SaveAsync(Vaccination vaccination);
    Task DeleteAsync(int id);
}

public interface IVisiteSanitaireRepository
{
    Task<List<VisiteSanitaire>> GetByPatientAsync(int patientId);
    Task<VisiteSanitaire> SaveAsync(VisiteSanitaire visite);
    Task DeleteAsync(int id);
}

public interface IIndisponibiliteRepository
{
    Task<List<Indisponibilite>> GetByPatientAsync(int patientId);
    Task<Indisponibilite> SaveAsync(Indisponibilite indispo);
    Task DeleteAsync(int id);
}

public interface ICertificatMedicalRepository
{
    Task<List<CertificatMedical>> GetByPatientAsync(int patientId);
    Task<CertificatMedical> SaveAsync(CertificatMedical certificat);
    Task DeleteAsync(int id);
}

public interface IDecisionReformeRepository
{
    Task<List<DecisionReformeMed>> GetByPatientAsync(int patientId);
    Task<DecisionReformeMed> SaveAsync(DecisionReformeMed decision);
    Task DeleteAsync(int id);
}

public interface IControleFinServiceRepository
{
    Task<ControleFinService?> GetByPatientAsync(int patientId);
    Task<ControleFinService> SaveAsync(ControleFinService controle);
}
