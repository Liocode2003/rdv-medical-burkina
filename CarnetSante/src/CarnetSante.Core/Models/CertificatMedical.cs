namespace CarnetSante.Core.Models;

public class CertificatMedical : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateCertificat { get; set; }
    public string TypeCertificat { get; set; } = string.Empty;
    public string Motif { get; set; } = string.Empty;
    public string MedecinRedacteur { get; set; } = string.Empty;
    public string Etablissement { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public string ReferenceNumero { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
