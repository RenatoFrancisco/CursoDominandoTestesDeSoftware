using DesenvolvedorIO.TesteDeSoftware.Demo;
using Xunit;

namespace DesenvolvedorIO.TesteDeSoftware.DemoTests
{
    public class CalculadoraTests
    {
        [Fact]
        public void Calculadora_Somar_RetonarValorSoma()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(2, 2);

            // Assert
            Assert.Equal(4, resultado);
        }

        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(2, 7, 9)]
        [InlineData(12, 4, 16)]
        [InlineData(25, 27, 52)]
        public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1, double v2, double total)
        {
            // Arange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(v1, v2);

            // Assert
            Assert.Equal(total, resultado);
        }
    }
}
