using CarnetSante.WPF.ViewModels.Auth;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Auth;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.ConnexionReussie += OnConnexionReussie;

        // Synchronisation PasswordBox (WPF sécurité : pas de binding direct)
        PasswordBox.PasswordChanged += (s, e) =>
            _viewModel.MotDePasse = PasswordBox.Password;

        LoginBox.Focus();
    }

    private void OnConnexionReussie()
    {
        // DialogResult ne peut être défini que si la fenêtre a été ouverte avec ShowDialog()
        if (IsModal()) DialogResult = true;
        Close();
    }

    private bool IsModal()
    {
        // Vérifie si la fenêtre a été ouverte modalement
        return (bool)typeof(Window)
            .GetField("_showingAsDialog",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(this)!;
    }
}
