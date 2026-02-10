using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

// Alias pour lever l'ambiguïté : le modèle ET l'enum s'appellent tous les deux DecisionReforme
using DecisionReformeEnum = CarnetSante.Core.Enums.DecisionReforme;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class DecisionReformeDialog : Window
{
    public DecisionReforme? Decision { get; private set; }   // modèle CarnetSante.Core.Models
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
        DecisionReformeEnum decisionEnum = decisionTag switch
        {
            "RefomeDefinitive"  => DecisionReformeEnum.RefomeDefinitive,
            "RefomeTemporaire"  => DecisionReformeEnum.RefomeTemporaire,
            "Reengagement"      => DecisionReformeEnum.Reengagement,
            "Reclassement"      => DecisionReformeEnum.Reclassement,
            _                   => DecisionReformeEnum.Maintien
        };

        Decision = new DecisionReforme
        {
            PatientId             = _patientId,
            DateDecision          = DateDecisionPicker.SelectedDate.Value,
            NumeroDecision        = NumeroDecisionBox.Text.Trim(),
            LieuCommission        = LieuCommissionBox.Text.Trim(),
            DateReunionCommission = DateReunionPicker.SelectedDate,
            CompositionCommission = CompositionBox.Text.Trim(),
            Diagnostic            = DiagnosticBox.Text.Trim(),
            CodeCIM10             = CodeCIM10Box.Text.Trim(),
            TauxInvalidite        = TauxInvaliditeBox.Text.Trim(),
            Decision              = decisionEnum,
            DateEffet             = DateEffetPicker.SelectedDate,
            PensionAttribuee      = PensionBox.IsChecked == true,
            TypePension           = TypePensionBox.Text.Trim(),
            Observations          = ObservationsBox.Text.Trim()
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
