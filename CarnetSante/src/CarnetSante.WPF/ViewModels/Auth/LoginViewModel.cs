using CarnetSante.Core.Services;

namespace CarnetSante.WPF.ViewModels.Auth;

/// <summary>
/// ViewModel de la fenêtre de connexion.
/// </summary>
public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IAuditService _auditService;

    public LoginViewModel(IAuthService authService, IAuditService auditService)
    {
        _authService = authService;
        _auditService = auditService;
        ConnecterCommand = new AsyncRelayCommand(ConnecterAsync, () => CanConnecter);
    }

    private string _login = string.Empty;
    public string Login
    {
        get => _login;
        set
        {
            SetProperty(ref _login, value);
            OnPropertyChanged(nameof(CanConnecter));
        }
    }

    private string _motDePasse = string.Empty;
    public string MotDePasse
    {
        get => _motDePasse;
        set
        {
            SetProperty(ref _motDePasse, value);
            OnPropertyChanged(nameof(CanConnecter));
        }
    }

    public bool CanConnecter => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(MotDePasse);

    public AsyncRelayCommand ConnecterCommand { get; }

    // Événement déclenché après connexion réussie
    public event Action? ConnexionReussie;

    private async Task ConnecterAsync()
    {
        ClearMessages();
        IsLoading = true;
        try
        {
            var user = await _authService.ConnecterAsync(Login, MotDePasse);
            if (user != null)
            {
                await _auditService.EnregistrerAsync(
                    CarnetSante.Core.Enums.TypeAction.Connexion,
                    $"Connexion de {user.Login}");
                ConnexionReussie?.Invoke();
            }
            else
            {
                ErrorMessage = "Login ou mot de passe incorrect. Vérifiez vos identifiants.";
            }
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Une erreur technique est survenue. Contactez l'administrateur.";
        }
        finally
        {
            IsLoading = false;
            MotDePasse = string.Empty;
        }
    }
}
