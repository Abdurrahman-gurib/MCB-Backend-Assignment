using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MCBBackend.Controllers;
using MCBBackend.Data;
using MCBBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MCBBackend.Tests
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly AppDbContext _context;

        public OrdersControllerTests()
        {
            // Create In-Memory database options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "OrdersTestDb")
                .Options;

            _context = new AppDbContext(options);
            var logger = new LoggerFactory().CreateLogger<OrdersController>();
            _controller = new OrdersController(_context, logger);
        }

        [Fact]
        public async Task GetOrders_ReturnsEmptyList_WhenNoOrders()
        {
            // Act
            var result = await _controller.GetOrders();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);

            // Assert
            Assert.Empty(orders);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var order = new Order
            {
                SupplierID = 1,
                OrderDate = System.DateTime.UtcNow,
                TotalAmount = 150.50m,
                Status = "Pending",
                Supplier = new Supplier
                {
                    SupplierName = "Test Supplier",
                    Address = "123 Test St",
                    ContactNo1 = "123-4567",
                    ContactNo2 = "987-6543"
                }
            };

            // Act
            var result = await _controller.CreateOrder(order);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedOrder = Assert.IsType<Order>(createdResult.Value);

            // Assert
            Assert.Equal("Pending", returnedOrder.Status);
            Assert.NotEqual(0, returnedOrder.OrderID); // ID should be generated
        }
    }
}
