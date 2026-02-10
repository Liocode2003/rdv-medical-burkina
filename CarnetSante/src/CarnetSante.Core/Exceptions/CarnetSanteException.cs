namespace CarnetSante.Core.Exceptions;

/// <summary>Exception de base de l'application.</summary>
public class CarnetSanteException : Exception
{
    public string? Code { get; }

    public CarnetSanteException(string message, string? code = null)
        : base(message) { Code = code; }

    public CarnetSanteException(string message, Exception inner, string? code = null)
        : base(message, inner) { Code = code; }
}

/// <summary>Exceptions liées à l'authentification.</summary>
public class AuthException : CarnetSanteException
{
    public AuthException(string message) : base(message, "AUTH_ERROR") { }
}

/// <summary>Accès non autorisé.</summary>
public class AccesNonAutoriseException : CarnetSanteException
{
    public AccesNonAutoriseException(string action = "cette action")
        : base($"Vous n'avez pas les droits pour effectuer {action}.", "ACCES_REFUSE") { }
}

/// <summary>Entité introuvable.</summary>
public class EntiteIntrouvableException : CarnetSanteException
{
    public EntiteIntrouvableException(string entite, int id)
        : base($"{entite} avec l'identifiant {id} est introuvable.", "NOT_FOUND") { }
}

/// <summary>Erreur de validation.</summary>
public class ValidationException : CarnetSanteException
{
    public IReadOnlyList<string> Erreurs { get; }

    public ValidationException(IEnumerable<string> erreurs)
        : base("Données invalides.")
    {
        Erreurs = erreurs.ToList().AsReadOnly();
    }

    public ValidationException(string erreur)
        : this(new[] { erreur }) { }
}
