namespace CarnetSante.Core.Enums;

/// <summary>Rôles utilisateurs du système.</summary>
public enum UserRole
{
    Administrateur = 1,
    Medecin = 2,
    Consultation = 3
}

/// <summary>Aptitude médicale d'un patient.</summary>
public enum AptitudeMedicale
{
    Apte = 1,
    Inapte = 2,
    ASurveiller = 3,
    ApteLimite = 4
}

/// <summary>Type d'intervention chirurgicale ou médicale.</summary>
public enum TypeIntervention
{
    Chirurgicale = 1,
    Medicale = 2,
    Diagnostique = 3,
    Rehabilitation = 4,
    Urgence = 5
}

/// <summary>Type de vaccin administré.</summary>
public enum TypeVaccin
{
    AntiAmaril = 1,
    AntiTetanique = 2,
    AntiMeningite = 3,
    AntiCovid = 4,
    AntiHepatiteB = 5,
    AntiPolio = 6,
    AntiRage = 7,
    AntiTyphoide = 8,
    Autre = 9
}

/// <summary>Statut d'indisponibilité.</summary>
public enum StatutIndisponibilite
{
    EnCours = 1,
    Terminee = 2,
    Prolongee = 3
}

/// <summary>Type de décision de commission de réforme.</summary>
public enum DecisionReforme
{
    Maintien = 1,
    RefomeDefinitive = 2,
    RefomeTemporaire = 3,
    Reengagement = 4,
    Reclassement = 5
}

/// <summary>Sexe du patient.</summary>
public enum Sexe
{
    Masculin = 1,
    Feminin = 2,
    Autre = 3
}

/// <summary>Groupe sanguin.</summary>
public enum GroupeSanguin
{
    APositif = 1,
    ANegatif = 2,
    BPositif = 3,
    BNegatif = 4,
    ABPositif = 5,
    ABNegatif = 6,
    OPositif = 7,
    ONegatif = 8,
    Inconnu = 9
}

/// <summary>Type d'action pour le journal d'audit.</summary>
public enum TypeAction
{
    Connexion = 1,
    Deconnexion = 2,
    CreationPatient = 3,
    ModificationPatient = 4,
    SuppressionPatient = 5,
    ConsultationDossier = 6,
    AjoutDonnee = 7,
    ModificationDonnee = 8,
    SuppressionDonnee = 9,
    Impression = 10,
    ExportPDF = 11,
    Sauvegarde = 12,
    Restauration = 13
}
