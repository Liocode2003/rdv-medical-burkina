using CarnetSante.Core.Models;

namespace CarnetSante.WPF.Services;

public interface IPdfService
{
    Task<string> ExportCarnetCompletAsync(Patient patient);
    Task<string> ExportConstantesAsync(Patient patient);
}

public interface INumeroCarnetService
{
    Task<string> GenerateAsync();
}
