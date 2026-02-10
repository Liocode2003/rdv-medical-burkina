using CarnetSante.WPF.ViewModels.Admin;
using System.Windows;

namespace CarnetSante.WPF.Views.Admin;

public partial class JournalAuditWindow : Window
{
    public JournalAuditWindow(JournalAuditViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
