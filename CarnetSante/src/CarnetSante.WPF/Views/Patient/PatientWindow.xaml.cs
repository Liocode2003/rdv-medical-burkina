using CarnetSante.WPF.ViewModels.Patient;
using System.Windows;

namespace CarnetSante.WPF.Views.Patient;

public partial class PatientWindow : Window
{
    public PatientViewModel ViewModel { get; }

    public PatientWindow(PatientViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = ViewModel;
    }

    private void BtnFermer_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.EstModifie)
        {
            var result = MessageBox.Show(
                "Des modifications non sauvegardées seront perdues. Voulez-vous quand même fermer ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;
        }
        Close();
    }

    private void BtnAjouterContact_Click(object sender, RoutedEventArgs e)
    {
        // Dialogue simple pour ajouter un contact d'urgence
        var dialog = new Modules.ContactUrgenceDialog(ViewModel.Patient?.EtatCivil?.Id ?? 0);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Contact != null)
        {
            ViewModel.Patient?.EtatCivil?.ContactsUrgence.Add(dialog.Contact);
            ViewModel.MarquerModifie();
        }
    }

    private void BtnNouvelleOperation_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.OperationDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Operation != null)
        {
            _ = ViewModel.Patient.OperationsMedicales.Append(dialog.Operation);
            ViewModel.MarquerModifie();
        }
    }
}
