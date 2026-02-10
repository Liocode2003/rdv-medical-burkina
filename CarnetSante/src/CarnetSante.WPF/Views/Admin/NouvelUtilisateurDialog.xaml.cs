using CarnetSante.Core.Enums;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Admin;

public partial class NouvelUtilisateurDialog : Window
{
    public string? Login     { get; private set; }
    public string  Nom       { get; private set; } = string.Empty;
    public string  Prenom    { get; private set; } = string.Empty;
    public string  Email     { get; private set; } = string.Empty;
    public UserRole Role     { get; private set; } = UserRole.Medecin;
    public string  MotDePasse { get; private set; } = string.Empty;

    public NouvelUtilisateurDialog()
    {
        InitializeComponent();
    }

    private void BtnCreer_Click(object sender, RoutedEventArgs e)
    {
        ErrorBorder.Visibility = Visibility.Collapsed;

        string login  = LoginBox.Text.Trim();
        string nom    = NomBox.Text.Trim();
        string prenom = PrenomBox.Text.Trim();
        string mdp    = MdpBox.Password;
        string mdpC   = MdpConfirmBox.Password;

        if (string.IsNullOrWhiteSpace(login))
        { ShowError("Le login est obligatoire."); return; }

        if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom))
        { ShowError("Le nom et les prénoms sont obligatoires."); return; }

        if (string.IsNullOrWhiteSpace(mdp) || mdp.Length < 8)
        { ShowError("Le mot de passe doit comporter au moins 8 caractères."); return; }

        if (mdp != mdpC)
        { ShowError("Les deux mots de passe ne correspondent pas."); return; }

        // Lecture du rôle sélectionné
        Role = RoleCombo.SelectedItem is ComboBoxItem item && item.Tag is string tag
            ? Enum.TryParse<UserRole>(tag, out var r) ? r : UserRole.Medecin
            : UserRole.Medecin;

        Login     = login;
        Nom       = nom;
        Prenom    = prenom;
        Email     = EmailBox.Text.Trim();
        MotDePasse = mdp;

        DialogResult = true;
        Close();
    }

    private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorBorder.Visibility = Visibility.Visible;
    }
}
