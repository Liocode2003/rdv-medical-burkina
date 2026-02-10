using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using System.Collections.ObjectModel;

namespace CarnetSante.WPF.ViewModels.Admin;

/// <summary>
/// ViewModel du journal d'audit (réservé administrateur).
/// </summary>
public class JournalAuditViewModel : BaseViewModel
{
    private readonly IAuditService _auditService;

    public JournalAuditViewModel(IAuditService auditService)
    {
        _auditService = auditService;

        ChargerCommand        = new AsyncRelayCommand(ChargerAsync);
        PageSuivanteCommand   = new AsyncRelayCommand(PageSuivanteAsync, () => !_estDernierePage);
        PagePrecedenteCommand = new AsyncRelayCommand(PagePrecedenteAsync, () => _page > 1);

        _ = ChargerAsync();
    }

    private ObservableCollection<JournalAudit> _entrees = new();
    public ObservableCollection<JournalAudit> Entrees
    {
        get => _entrees;
        set => SetProperty(ref _entrees, value);
    }

    private int _page = 1;
    public int Page
    {
        get => _page;
        set
        {
            SetProperty(ref _page, value);
            OnPropertyChanged(nameof(PageLabel));
        }
    }

    public string PageLabel => $"Page {_page}";

    private bool _estDernierePage;

    public AsyncRelayCommand ChargerCommand        { get; }
    public AsyncRelayCommand PageSuivanteCommand   { get; }
    public AsyncRelayCommand PagePrecedenteCommand { get; }

    private async Task ChargerAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            const int taille = 50;
            var entrees = (await _auditService.GetJournalAsync(_page, taille)).ToList();
            Entrees = new ObservableCollection<JournalAudit>(entrees);
            _estDernierePage = entrees.Count < taille;
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }

    private async Task PageSuivanteAsync()
    {
        Page++;
        await ChargerAsync();
    }

    private async Task PagePrecedenteAsync()
    {
        if (_page > 1) Page--;
        await ChargerAsync();
    }
}
