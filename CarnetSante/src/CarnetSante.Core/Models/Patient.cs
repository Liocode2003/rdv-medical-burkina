using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class Patient : BaseEntity
{
    public string NumeroCarnet { get; set; } = string.Empty;
    public string Matricule { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenoms { get; set; } = string.Empty;
    public DateTime DateNaissance { get; set; }
    public string LieuNaissance { get; set; } = string.Empty;
    public Sexe Sexe { get; set; }
    public GroupeSanguin GroupeSanguin { get; set; }
    public string NationaliteIFO { get; set; } = "Burkinabé";
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string NomComplet => $"{Nom} {Prenoms}";
    public int Age => DateTime.Now.Year - DateNaissance.Year;

    // Navigation properties
    public EtatCivil? EtatCivil { get; set; }
    public ICollection<ContactUrgence> ContactsUrgence { get; set; } = new List<ContactUrgence>();
    public ICollection<Constante> Constantes { get; set; } = new List<Constante>();
    public ExamenIncorporation? ExamenIncorporation { get; set; }
    public ICollection<OperationMedicale> OperationsMedicales { get; set; } = new List<OperationMedicale>();
    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
    public ICollection<VisiteSanitaire> VisitesSanitaires { get; set; } = new List<VisiteSanitaire>();
    public ICollection<Indisponibilite> Indisponibilites { get; set; } = new List<Indisponibilite>();
    public ICollection<CertificatMedical> CertificatsMedicaux { get; set; } = new List<CertificatMedical>();
    public ICollection<DecisionReformeMed> DecisionsReforme { get; set; } = new List<DecisionReformeMed>();
    public ControleFinService? ControleFinService { get; set; }
}
