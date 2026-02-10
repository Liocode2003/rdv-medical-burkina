namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE B - CONSTANTES BIOLOGIQUES ET ANTHROPOMÉTRIQUES
/// Chaque mesure est horodatée pour permettre le suivi longitudinal.
/// </summary>
public class Constante : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateMesure { get; set; }

    // ── Anthropométrie ──────────────────────────────────────
    /// <summary>Taille en centimètres.</summary>
    public decimal? Taille { get; set; }

    /// <summary>Poids en kilogrammes.</summary>
    public decimal? Poids { get; set; }

    /// <summary>Indice de Masse Corporelle (calculé automatiquement).</summary>
    public decimal? IMC { get; set; }

    /// <summary>Périmètre thoracique en centimètres.</summary>
    public decimal? PerimethreThoracique { get; set; }

    /// <summary>Périmètre abdominal en centimètres.</summary>
    public decimal? PerimetreAbdominal { get; set; }

    // ── Signes vitaux ────────────────────────────────────────
    /// <summary>Tension artérielle systolique (mmHg).</summary>
    public int? TensionSystolique { get; set; }

    /// <summary>Tension artérielle diastolique (mmHg).</summary>
    public int? TensionDiastolique { get; set; }

    /// <summary>Fréquence cardiaque (bpm).</summary>
    public int? FrequenceCardiaque { get; set; }

    /// <summary>Fréquence respiratoire (cycles/min).</summary>
    public int? FrequenceRespiratoire { get; set; }

    /// <summary>Température corporelle en degrés Celsius.</summary>
    public decimal? Temperature { get; set; }

    // ── Analyses biologiques ─────────────────────────────────
    /// <summary>Glycémie en g/L ou mmol/L.</summary>
    public decimal? Glycemie { get; set; }

    /// <summary>Unité de mesure de la glycémie (g/L ou mmol/L).</summary>
    public string? UnitéGlycemie { get; set; } = "g/L";

    /// <summary>Albumine sérique en g/L.</summary>
    public decimal? Albumine { get; set; }

    /// <summary>Saturation en oxygène (SpO2) en %.</summary>
    public decimal? Spo2 { get; set; }

    // ── Contexte ─────────────────────────────────────────────
    public string? MedecinMesureur { get; set; }
    public string? Observations { get; set; }

    // Navigation
    public Patient? Patient { get; set; }

    // ── Calcul automatique de l'IMC ───────────────────────────
    public decimal? CalculerIMC()
    {
        if (Taille.HasValue && Poids.HasValue && Taille.Value > 0)
        {
            var tailleM = Taille.Value / 100m;
            return Math.Round(Poids.Value / (tailleM * tailleM), 1);
        }
        return null;
    }
}
