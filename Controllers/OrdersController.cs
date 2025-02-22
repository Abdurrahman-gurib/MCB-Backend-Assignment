using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MCBBackend.Data;
using MCBBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MCBBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            _logger.LogInformation("Fetching orders");
            var orders = await _context.Orders.Include(o => o.Supplier).ToListAsync();
            _logger.LogInformation("Fetched {Count} orders", orders.Count);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _logger.LogInformation("Creating a new order for supplier {SupplierID}", order.SupplierID);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Order created with ID {OrderID}", order.OrderID);
            return CreatedAtAction(nameof(GetOrders), new { id = order.OrderID }, order);
        }
    }
}
