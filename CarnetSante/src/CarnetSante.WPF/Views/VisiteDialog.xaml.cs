using CarnetSante.Core.Models;
using System.Windows;

namespace CarnetSante.WPF.Views;

public partial class VisiteDialog : Window
{
    public VisiteSanitaire Visite { get; set; }
    public bool IsSaved { get; private set; }

    public VisiteDialog(VisiteSanitaire visite)
    {
        InitializeComponent();
        Visite = visite;
        DataContext = Visite;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(Visite.Motif))
        {
            MessageBox.Show("Le motif d'hospitalisation est obligatoire.",
                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(Visite.Etablissement))
        {
            MessageBox.Show("L'établissement est obligatoire.",
                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsSaved = true;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        IsSaved = false;
        DialogResult = false;
        Close();
    }
}
