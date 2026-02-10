using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class VisiteDialog : Window
{
    public VisiteSanitaire? Visite { get; private set; }
    private readonly int _patientId;

    public VisiteDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateVisitePicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (DateVisitePicker.SelectedDate == null)
        {
            MessageBox.Show("La date de la visite est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(EntiteMedicaleBox.Text))
        {
            MessageBox.Show("L'entité médicale est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Visite = new VisiteSanitaire
        {
            PatientId               = _patientId,
            DateVisite              = DateVisitePicker.SelectedDate.Value,
            TypeVisite              = (TypeVisiteBox.SelectedItem as ComboBoxItem)?.Content?.ToString(),
            EntiteMedicale          = EntiteMedicaleBox.Text.Trim(),
            NomMedecin              = MedecinBox.Text.Trim(),
            ResultatsVisite         = ResultatsBox.Text.Trim(),
            DiagnosticsRetenus      = DiagnosticsBox.Text.Trim(),
            ExamensPrescrits        = ExamensBox.Text.Trim(),
            TraitementsPrescrits    = TraitementsBox.Text.Trim(),
            AptitudeConclusionVisite = AptitudeBox.Text.Trim(),
            ProchainContrôle        = ProchainControlePicker.SelectedDate,
            Observations            = ObservationsBox.Text.Trim()
        };

        DialogResult = true;
        Close();
    }

    private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
