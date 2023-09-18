using Xunit;
using CalculationSite.Services;

namespace CalculationSite.Tests.Services
{
    public class CalculationServiceTest
    {
        CalculationService calculationService;
        public CalculationServiceTest() 
        {
            calculationService = new CalculationService();   
        }

        [Fact]
        public async void Calculate_EmptyCollection()
        {
            // Arrange
            int[] emptyCollection = Array.Empty<int>();

            // Act
            var result = await calculationService.Calculate(emptyCollection);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async void Calculate_Collection_CorrectAnswer()
        {
            // Arrange
            int[] collection = new int[3] { 1, 2, 3 };

            // Act
            var result = await calculationService.Calculate(collection);

            // Assert
            Assert.Equal(14, result);
        }

        [Fact]
        public async void Calculate_Collection_WrongAnswer()
        {
            // Arrange
            int[] collection = new int[3] { 1, 2, 3 };

            // Act
            var result = await calculationService.Calculate(collection);

            // Assert
            Assert.False(0 == result);
        }
    }
}
