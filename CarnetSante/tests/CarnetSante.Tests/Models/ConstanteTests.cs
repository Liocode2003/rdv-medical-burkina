using CarnetSante.Core.Models;
using FluentAssertions;
using Xunit;

namespace CarnetSante.Tests.Models;

/// <summary>
/// Tests du modèle Constante (calcul IMC et règles métier).
/// </summary>
public class ConstanteTests
{
    [Theory]
    [InlineData(170, 70, 24.2)]   // Poids normal
    [InlineData(180, 90, 27.8)]   // Surpoids
    [InlineData(165, 50, 18.4)]   // Insuffisance pondérale
    [InlineData(175, 120, 39.2)]  // Obésité sévère
    public void CalculerIMC_ShouldReturnCorrectValue(decimal tailleCm, decimal poidsKg, decimal imcAttendu)
    {
        // Arrange
        var constante = new Constante { Taille = tailleCm, Poids = poidsKg };

        // Act
        var imc = constante.CalculerIMC();

        // Assert
        imc.Should().NotBeNull();
        imc!.Value.Should().BeApproximately(imcAttendu, 0.1m);
    }

    [Fact]
    public void CalculerIMC_ShouldReturnNullWhenTailleIsNull()
    {
        // Arrange
        var constante = new Constante { Poids = 70 };

        // Act
        var imc = constante.CalculerIMC();

        // Assert
        imc.Should().BeNull();
    }

    [Fact]
    public void CalculerIMC_ShouldReturnNullWhenPoidsIsNull()
    {
        // Arrange
        var constante = new Constante { Taille = 170 };

        // Act
        var imc = constante.CalculerIMC();

        // Assert
        imc.Should().BeNull();
    }

    [Fact]
    public void CalculerIMC_ShouldReturnNullWhenTailleIsZero()
    {
        // Arrange
        var constante = new Constante { Taille = 0, Poids = 70 };

        // Act
        var imc = constante.CalculerIMC();

        // Assert
        imc.Should().BeNull();
    }

    [Fact]
    public void CalculerIMC_ShouldRoundToOneDecimal()
    {
        // Arrange : 175cm, 75kg → IMC = 75 / 1.75² = 24.489... → arrondi à 24.5
        var constante = new Constante { Taille = 175, Poids = 75 };

        // Act
        var imc = constante.CalculerIMC();

        // Assert
        imc.Should().NotBeNull();
        // Vérifie que l'arrondi est correct (1 décimale)
        var decimals = imc!.Value.ToString().Split('.');
        if (decimals.Length > 1)
            decimals[1].Length.Should().BeLessOrEqualTo(1);
    }
}
