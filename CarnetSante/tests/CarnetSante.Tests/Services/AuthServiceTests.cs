using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using CarnetSante.Data.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarnetSante.Tests.Services;

/// <summary>
/// Tests du service d'authentification.
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IUtilisateurRepository> _repoMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _repoMock = new Mock<IUtilisateurRepository>();
        _authService = new AuthService(_repoMock.Object);
    }

    [Fact]
    public void HacherMotDePasse_ShouldReturnBCryptHash()
    {
        // Act
        var hash = _authService.HacherMotDePasse("MonMotDePasse123!");

        // Assert
        hash.Should().StartWith("$2a$");
        hash.Should().HaveLength(60);
    }

    [Fact]
    public void VerifierMotDePasse_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        var motDePasse = "MonMotDePasse123!";
        var hash = _authService.HacherMotDePasse(motDePasse);

        // Act
        var resultat = _authService.VerifierMotDePasse(motDePasse, hash);

        // Assert
        resultat.Should().BeTrue();
    }

    [Fact]
    public void VerifierMotDePasse_ShouldReturnFalseForWrongPassword()
    {
        // Arrange
        var hash = _authService.HacherMotDePasse("MotDePasseOriginal");

        // Act
        var resultat = _authService.VerifierMotDePasse("MauvaisMotDePasse", hash);

        // Assert
        resultat.Should().BeFalse();
    }

    [Fact]
    public async Task ConnecterAsync_ShouldReturnNullForUnknownUser()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByLoginAsync("inconnu"))
            .ReturnsAsync((Utilisateur?)null);

        // Act
        var result = await _authService.ConnecterAsync("inconnu", "motdepasse");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ConnecterAsync_ShouldReturnNullForWrongPassword()
    {
        // Arrange
        var hash = _authService.HacherMotDePasse("BonMotDePasse");
        var utilisateur = new Utilisateur
        {
            Id = 1, Login = "admin", MotDePasseHash = hash,
            Role = UserRole.Administrateur, EstActif = true
        };
        _repoMock.Setup(r => r.GetByLoginAsync("admin")).ReturnsAsync(utilisateur);
        _repoMock.Setup(r => r.IncrémenterTentativesEchecAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _authService.ConnecterAsync("admin", "MauvaisMotDePasse");

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.IncrémenterTentativesEchecAsync(1), Times.Once);
    }

    [Fact]
    public async Task ConnecterAsync_ShouldReturnUserForCorrectCredentials()
    {
        // Arrange
        var motDePasse = "Admin@2024!";
        var hash = _authService.HacherMotDePasse(motDePasse);
        var utilisateur = new Utilisateur
        {
            Id = 1, Login = "admin", MotDePasseHash = hash,
            Role = UserRole.Administrateur, EstActif = true
        };
        _repoMock.Setup(r => r.GetByLoginAsync("admin")).ReturnsAsync(utilisateur);
        _repoMock.Setup(r => r.RéinitialiserTentativesAsync(1)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.MettreAJourDerniereConnexionAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _authService.ConnecterAsync("admin", motDePasse);

        // Assert
        result.Should().NotBeNull();
        result!.Login.Should().Be("admin");
        _authService.UtilisateurCourant.Should().Be(result);
    }

    [Fact]
    public async Task ConnecterAsync_ShouldThrowForLockedAccount()
    {
        // Arrange
        var utilisateur = new Utilisateur
        {
            Id = 1, Login = "locked", MotDePasseHash = "hash",
            BloquéJusquau = DateTime.UtcNow.AddMinutes(15)
        };
        _repoMock.Setup(r => r.GetByLoginAsync("locked")).ReturnsAsync(utilisateur);

        // Act & Assert
        await _authService.Invoking(s => s.ConnecterAsync("locked", "pwd"))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*bloqué*");
    }

    [Fact]
    public void ARole_ShouldReturnFalseWhenNotConnected()
    {
        // Act
        var result = _authService.ARole(UserRole.Administrateur);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeconnecterAsync_ShouldClearCurrentUser()
    {
        // Arrange : connecter d'abord
        var motDePasse = "Test@1234";
        var hash = _authService.HacherMotDePasse(motDePasse);
        var utilisateur = new Utilisateur
        {
            Id = 2, Login = "medecin", MotDePasseHash = hash,
            Role = UserRole.Medecin, EstActif = true
        };
        _repoMock.Setup(r => r.GetByLoginAsync("medecin")).ReturnsAsync(utilisateur);
        _repoMock.Setup(r => r.RéinitialiserTentativesAsync(2)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.MettreAJourDerniereConnexionAsync(2)).Returns(Task.CompletedTask);
        await _authService.ConnecterAsync("medecin", motDePasse);

        // Act
        await _authService.DeconnecterAsync();

        // Assert
        _authService.UtilisateurCourant.Should().BeNull();
    }
}
