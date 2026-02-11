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

        // Synchronisation PasswordBox (WPF sécurité : pas de binding direct)
        PasswordBox.PasswordChanged += (s, e) =>
            _viewModel.MotDePasse = PasswordBox.Password;

        LoginBox.Focus();
    }


}
