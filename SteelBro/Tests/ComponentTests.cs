using System.ComponentModel.DataAnnotations;
using SteelBro.Models;
using Xunit;

namespace SteelBro.Tests.Models
{
    public class ComponentTests
    {
        [Fact]
        public void Component_Properties_SetCorrectly()
        {
            var component = new Component
            {
                ComponentId = 1,
                ComponentName = "Test Component",
                Price = 100.50m,
                Quantity = 10,
                Specifications = "Test specifications"
            };

            Assert.Equal(1, component.ComponentId);
            Assert.Equal("Test Component", component.ComponentName);
            Assert.Equal(100.50m, component.Price);
            Assert.Equal(10, component.Quantity);
            Assert.Equal("Test specifications", component.Specifications);
        }
        [Fact]
        public void Component_InvalidData_FailsValidation()
        {
            // Arrange
            var component = new Component
            {
                ComponentId = 1,
                ComponentName = null, // Отсутствует обязательное свойство
                Price = -100.00m, // Некорректное значение (отрицательная цена)
                Quantity = 0 // Некорректное значение (количество не может быть 0)
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(component);
            var isValid = Validator.TryValidateObject(component, context, validationResults, true);

            Assert.False(isValid); 
            Assert.Equal(3, validationResults.Count); 

            Assert.Contains("Поле Название комплектующего обязательно для заполнения.", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Цена должна быть больше или равна 0.", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Количество должно быть больше 0.", validationResults.Select(r => r.ErrorMessage));
        }
    }
}