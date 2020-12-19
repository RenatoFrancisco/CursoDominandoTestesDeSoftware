using DesenvolvedorIO.TesteDeSoftware.Demo;
using Xunit;

namespace DesenvolvedorIO.TesteDeSoftware.DemoTests
{
    public class AssertNumbersTests
    {
        [Fact]
        public void Calculadora_Somar_DeveSerIgual()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var result = calculadora.Somar(1, 2);

            // Assert
            Assert.Equal(3, result);
        }
        
        [Fact]
        public void Calculadora_Somar_NaoDeveSerIgual()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var result = calculadora.Somar(1.13123123123, 2.2312313123);

            // Assert
            Assert.NotEqual(3.3, result,1);
        }
    }
}