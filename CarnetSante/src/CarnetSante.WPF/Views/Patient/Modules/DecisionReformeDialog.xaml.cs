using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class DecisionReformeDialog : Window
{
    public DecisionReforme? Decision { get; private set; }
    private readonly int _patientId;

    public DecisionReformeDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateDecisionPicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (DateDecisionPicker.SelectedDate == null)
        {
            MessageBox.Show("La date de la décision est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(NumeroDecisionBox.Text))
        {
            MessageBox.Show("Le numéro de la décision est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(DiagnosticBox.Text))
        {
            MessageBox.Show("Le diagnostic est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var decisionTag = (DecisionBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Maintien";
        Enums.TypeDecisionReforme decisionEnum = decisionTag switch
        {
            "RefomeDefinitive"  => Enums.TypeDecisionReforme.RefomeDefinitive,
            "RefomeTemporaire"  => Enums.TypeDecisionReforme.RefomeTemporaire,
            "Reengagement"      => Enums.TypeDecisionReforme.Reengagement,
            "Reclassement"      => Enums.TypeDecisionReforme.Reclassement,
            _                   => Enums.TypeDecisionReforme.Maintien
        };

        Decision = new DecisionReforme
        {
            PatientId            = _patientId,
            DateDecision         = DateDecisionPicker.SelectedDate.Value,
            NumeroDecision       = NumeroDecisionBox.Text.Trim(),
            LieuCommission       = LieuCommissionBox.Text.Trim(),
            DateReunionCommission = DateReunionPicker.SelectedDate,
            CompositionCommission = CompositionBox.Text.Trim(),
            Diagnostic           = DiagnosticBox.Text.Trim(),
            CodeCIM10            = CodeCIM10Box.Text.Trim(),
            TauxInvalidite       = TauxInvaliditeBox.Text.Trim(),
            Decision             = decisionEnum,
            DateEffet            = DateEffetPicker.SelectedDate,
            PensionAttribuee     = PensionBox.IsChecked == true,
            TypePension          = TypePensionBox.Text.Trim(),
            Observations         = ObservationsBox.Text.Trim()
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
