using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE E - VACCINATIONS ET IMMUNISATIONS
/// Historique complet du calendrier vaccinal.
/// </summary>
public class Vaccination : BaseEntity
{
    public int PatientId { get; set; }

    public TypeVaccin TypeVaccin { get; set; }
    public string NomVaccin { get; set; } = string.Empty;   // Nom commercial ou générique
    public string? FabricantVaccin { get; set; }
    public string? NumeroLot { get; set; }
    public string? Reference { get; set; }

    // ── Protocole ────────────────────────────────────────────
    public DateTime DateVaccination { get; set; }
    public int NumeroDose { get; set; } = 1;            // Primovaccination, rappel 1, 2...
    public string? TypeDose { get; set; }               // Dose complète, demi-dose, rappel
    public decimal? QuantitéDose { get; set; }          // En ml
    public string? VoieAdministration { get; set; }     // SC, IM, ID, PO
    public string? SiteInjection { get; set; }          // Bras droit, bras gauche...
    public DateTime? ProchaineInjectionPrevue { get; set; }

    // ── Validité ─────────────────────────────────────────────
    public DateTime? DateExpiration { get; set; }       // Expiration de la protection
    public bool EstValide { get; set; } = true;

    // ── Réaction ─────────────────────────────────────────────
    public string? ReactionPostVaccinale { get; set; }
    public string? Observations { get; set; }

    // ── Médecin vaccinateur ──────────────────────────────────
    public string? MedecinVaccinateur { get; set; }
    public string? CodeMedecin { get; set; }
    public string? Etablissement { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
