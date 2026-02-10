using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class OperationDialog : Window
{
    public OperationMedicale? Operation { get; private set; }
    private readonly int _patientId;

    public OperationDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateOperationPicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DiagnosticBox.Text))
        {
            MessageBox.Show("Le diagnostic est obligatoire.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (DateOperationPicker.SelectedDate == null)
        {
            MessageBox.Show("La date est obligatoire.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var typeStr = (TypeInterventionBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Médicale";
        TypeIntervention type = typeStr switch
        {
            "Chirurgicale" => TypeIntervention.Chirurgicale,
            "Diagnostique" => TypeIntervention.Diagnostique,
            "Rééducation" => TypeIntervention.Rehabilitation,
            "Urgence" => TypeIntervention.Urgence,
            _ => TypeIntervention.Medicale
        };

        Operation = new OperationMedicale
        {
            PatientId = _patientId,
            DateOperation = DateOperationPicker.SelectedDate.Value,
            Diagnostic = DiagnosticBox.Text.Trim(),
            TypeIntervention = type,
            LieuSejour = LieuSejourBox.Text.Trim(),
            SignatureMedecin = MedecinBox.Text.Trim(),
            EtatAvant = EtatAvantBox.Text.Trim(),
            EtatApres = EtatApresBox.Text.Trim(),
            Observations = ObservationsBox.Text.Trim()
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
