using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class IndisponibiliteDialog : Window
{
    public Indisponibilite? Indisponibilite { get; private set; }
    private readonly int _patientId;

    public IndisponibiliteDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateDebutPicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (DateDebutPicker.SelectedDate == null)
        {
            MessageBox.Show("La date de début est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(MotifBox.Text))
        {
            MessageBox.Show("Le motif est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!int.TryParse(DureePrescBox.Text, out var duree) || duree <= 0)
        {
            MessageBox.Show("La durée prescrite doit être un nombre entier positif.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var statutTag = (StatutBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "EnCours";
        StatutIndisponibilite statut = statutTag switch
        {
            "Terminee"  => StatutIndisponibilite.Terminee,
            "Prolongee" => StatutIndisponibilite.Prolongee,
            _           => StatutIndisponibilite.EnCours
        };

        var typeSejourStr = (TypeSejourBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

        Indisponibilite = new Indisponibilite
        {
            PatientId          = _patientId,
            DateDebut          = DateDebutPicker.SelectedDate.Value,
            DateFin            = DateFinPicker.SelectedDate,
            DureePrescrite     = duree,
            Statut             = statut,
            Motif              = MotifBox.Text.Trim(),
            Diagnostic         = DiagnosticBox.Text.Trim(),
            CodeCIM10          = CodeCIM10Box.Text.Trim(),
            LieuSejour         = LieuSejourBox.Text.Trim(),
            TypeSejour         = typeSejourStr,
            EtatDepart         = EtatDepartBox.Text.Trim(),
            EtatRetour         = EtatRetourBox.Text.Trim(),
            MedecinPrescripteur = MedecinBox.Text.Trim(),
            Observations       = ObservationsBox.Text.Trim()
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
