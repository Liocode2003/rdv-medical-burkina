using CarnetSante.Core.Models;

namespace CarnetSante.Core.Interfaces;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllAsync(string? searchTerm = null);
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient?> GetByNumeroCarnetAsync(string numeroCarnet);
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    Task DeleteAsync(int id);
    Task<int> GetCountAsync();
    Task<bool> ExistsAsync(string numeroCarnet, int? excludeId = null);
}
