using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using CarnetSante.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarnetSante.Tests.Repositories;

/// <summary>
/// Tests du repository Patient avec base de données en mémoire.
/// </summary>
public class PatientRepositoryTests : IDisposable
{
    private readonly CarnetSanteDbContext _context;
    private readonly PatientRepository _repository;

    public PatientRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CarnetSanteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarnetSanteDbContext(options);
        _repository = new PatientRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistPatient()
    {
        // Arrange
        var patient = new Patient { NumeroCarnet = "CS-2024-00001" };

        // Act
        var result = await _repository.AddAsync(patient);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.NumeroCarnet.Should().Be("CS-2024-00001");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectPatient()
    {
        // Arrange
        var patient = new Patient { NumeroCarnet = "CS-2024-00002" };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.NumeroCarnet.Should().Be("CS-2024-00002");
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeletePatient()
    {
        // Arrange
        var patient = new Patient { NumeroCarnet = "CS-2024-00003" };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(patient.Id);

        // Assert : le patient ne doit plus être trouvé via GetByIdAsync (filtre IsDeleted)
        var found = await _repository.GetByIdAsync(patient.Id);
        found.Should().BeNull();

        // Mais il doit toujours exister en base (soft delete)
        var raw = await _context.Patients.IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == patient.Id);
        raw.Should().NotBeNull();
        raw!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task NumeroCarnetExisteAsync_ShouldReturnTrueForExistingNumber()
    {
        // Arrange
        var patient = new Patient { NumeroCarnet = "CS-2024-99999" };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var existe = await _repository.NumeroCarnetExisteAsync("CS-2024-99999");

        // Assert
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task NumeroCarnetExisteAsync_ShouldReturnFalseForNewNumber()
    {
        // Act
        var existe = await _repository.NumeroCarnetExisteAsync("CS-INCONNU-00000");

        // Assert
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task RechercherAsync_ShouldFilterByNumeroCarnet()
    {
        // Arrange
        await _context.Patients.AddRangeAsync(
            new Patient { NumeroCarnet = "CS-2024-10001" },
            new Patient { NumeroCarnet = "CS-2024-10002" },
            new Patient { NumeroCarnet = "CS-2025-00001" }
        );
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.RechercherAsync(null, null, "2024", null);

        // Assert
        results.Should().HaveCount(2);
        results.Should().AllSatisfy(p => p.NumeroCarnet.Should().Contain("2024"));
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
