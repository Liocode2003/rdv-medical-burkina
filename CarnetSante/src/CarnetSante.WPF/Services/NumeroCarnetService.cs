using CarnetSante.Core.Interfaces;

namespace CarnetSante.WPF.Services;

public class NumeroCarnetService : INumeroCarnetService
{
    private readonly IPatientRepository _repo;

    public NumeroCarnetService(IPatientRepository repo)
    {
        _repo = repo;
    }

    public async Task<string> GenerateAsync()
    {
        var count = await _repo.GetCountAsync();
        var year = DateTime.Now.Year;
        var seq = (count + 1).ToString("D5");
        return $"CS-{year}-{seq}";
    }
}
