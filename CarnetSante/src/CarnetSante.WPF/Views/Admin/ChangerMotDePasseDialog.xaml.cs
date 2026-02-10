using CarnetSante.Core.Services;
using System.Windows;

namespace CarnetSante.WPF.Views.Admin;

public partial class ChangerMotDePasseDialog : Window
{
    private readonly IAuthService _authService;

    public ChangerMotDePasseDialog(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void BtnChanger_Click(object sender, RoutedEventArgs e)
    {
        ErrorBorder.Visibility  = Visibility.Collapsed;
        SuccessBorder.Visibility = Visibility.Collapsed;

        string ancien  = AncienMdpBox.Password;
        string nouveau = NouveauMdpBox.Password;
        string confirm = ConfirmMdpBox.Password;

        if (string.IsNullOrWhiteSpace(ancien))
        { ShowError("Veuillez saisir votre mot de passe actuel."); return; }

        if (string.IsNullOrWhiteSpace(nouveau) || nouveau.Length < 8)
        { ShowError("Le nouveau mot de passe doit comporter au moins 8 caractères."); return; }

        if (nouveau != confirm)
        { ShowError("Les deux nouveaux mots de passe ne correspondent pas."); return; }

        var userId = _authService.UtilisateurCourant?.Id ?? 0;
        bool ok = await _authService.ChangerMotDePasseAsync(userId, ancien, nouveau);

        if (ok)
        {
            SuccessText.Text     = "Mot de passe changé avec succès !";
            SuccessBorder.Visibility = Visibility.Visible;
            AncienMdpBox.Clear();
            NouveauMdpBox.Clear();
            ConfirmMdpBox.Clear();
        }
        else
        {
            ShowError("Mot de passe actuel incorrect. Réessayez.");
        }
    }

    private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorBorder.Visibility = Visibility.Visible;
    }
}
