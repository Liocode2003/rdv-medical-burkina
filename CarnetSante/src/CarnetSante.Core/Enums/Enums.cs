namespace CarnetSante.Core.Enums;

public enum Sexe { Masculin, Feminin }

public enum GroupeSanguin { AP, AM, BP, BM, ABP, ABM, OP, OM, Inconnu }

public enum Aptitude { Apte, Inapte, ApteLimite, ASurveiller }

public enum TypeVaccin
{
    AntiAmaril, AntiTetanique, AntiMeningite, AntiCovid,
    AntiHepatiteB, AntiPolio, AntiRage, AntiTyphoide, Autre
}

public enum TypeIntervention
{
    Chirurgicale, Medicale, Diagnostique, Rehabilitation, Urgence
}

public enum DecisionReforme
{
    Maintien, ReformeDefinitive, ReformeTemporaire, Reengagement, Reclassement
}

public enum StatutDocument { Actif, Archive, Supprime }
