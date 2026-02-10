using CarnetSante.Core.Enums;
using CarnetSante.WPF.ViewModels.Admin;
using System.Windows;

namespace CarnetSante.WPF.Views.Admin;

public partial class GestionUtilisateursWindow : Window
{
    private readonly GestionUtilisateursViewModel _viewModel;

    public GestionUtilisateursWindow(GestionUtilisateursViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.DemanderCreation += OnDemanderCreation;
    }

    private void BtnNouvelUtilisateur_Click(object sender, RoutedEventArgs e)
    {
        OnDemanderCreation();
    }

    private void OnDemanderCreation()
    {
        var dialog = new NouvelUtilisateurDialog();
        dialog.Owner = this;

        if (dialog.ShowDialog() == true && dialog.Login != null)
        {
            _ = _viewModel.AjouterUtilisateurAsync(
                dialog.Login,
                dialog.Nom,
                dialog.Prenom,
                dialog.Email,
                dialog.Role,
                dialog.MotDePasse);
        }
    }
}
