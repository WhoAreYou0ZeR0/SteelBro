using SteelBro.Models;
using Xunit;

namespace SteelBro.Tests.Models
{
    public class OrderTests
    {
        [Fact]
        public void Order_Properties_SetCorrectly()
        {
            var order = new Order
            {
                OrderId = 1,
                ClientId = 1,
                ServiceId = 2,
                StatusId = 3,
                WorkerId = 4,
                OrderDate = DateTime.Now,
                ExDate = DateTime.Now.AddDays(7),
                Comment = "Test comment"
            };

            Assert.Equal(1, order.OrderId);
            Assert.Equal(1, order.ClientId);
            Assert.Equal(2, order.ServiceId);
            Assert.Equal(3, order.StatusId);
            Assert.Equal(4, order.WorkerId);
            Assert.NotNull(order.OrderDate);
            Assert.NotNull(order.ExDate);
            Assert.Equal("Test comment", order.Comment);
        }
    }
}