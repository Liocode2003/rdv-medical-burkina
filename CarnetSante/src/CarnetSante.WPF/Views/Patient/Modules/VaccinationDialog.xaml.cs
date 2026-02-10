using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class VaccinationDialog : Window
{
    public Vaccination? Vaccination { get; private set; }
    private readonly int _patientId;

    public VaccinationDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateVaccinationPicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NomVaccinBox.Text))
        {
            MessageBox.Show("Le nom du vaccin est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (DateVaccinationPicker.SelectedDate == null)
        {
            MessageBox.Show("La date de vaccination est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var typeTag = (TypeVaccinBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Autre";
        TypeVaccin type = typeTag switch
        {
            "AntiAmaril"    => TypeVaccin.AntiAmaril,
            "AntiTetanique" => TypeVaccin.AntiTetanique,
            "AntiMeningite" => TypeVaccin.AntiMeningite,
            "AntiCovid"     => TypeVaccin.AntiCovid,
            "AntiHepatiteB" => TypeVaccin.AntiHepatiteB,
            "AntiPolio"     => TypeVaccin.AntiPolio,
            "AntiRage"      => TypeVaccin.AntiRage,
            "AntiTyphoide"  => TypeVaccin.AntiTyphoide,
            _               => TypeVaccin.Autre
        };

        _ = int.TryParse(NumeroDoseBox.Text, out var numeroDose);

        Vaccination = new Vaccination
        {
            PatientId             = _patientId,
            TypeVaccin            = type,
            NomVaccin             = NomVaccinBox.Text.Trim(),
            FabricantVaccin       = FabricantBox.Text.Trim(),
            NumeroLot             = NumeroLotBox.Text.Trim(),
            DateVaccination       = DateVaccinationPicker.SelectedDate.Value,
            NumeroDose            = numeroDose > 0 ? numeroDose : 1,
            VoieAdministration    = (VoieAdminBox.SelectedItem as ComboBoxItem)?.Content?.ToString(),
            SiteInjection         = SiteInjectionBox.Text.Trim(),
            ProchaineInjectionPrevue = ProchaineInjectionPicker.SelectedDate,
            MedecinVaccinateur    = MedecinBox.Text.Trim(),
            Etablissement         = EtablissementBox.Text.Trim(),
            ReactionPostVaccinale = ReactionBox.Text.Trim(),
            Observations          = ObservationsBox.Text.Trim(),
            EstValide             = true
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
