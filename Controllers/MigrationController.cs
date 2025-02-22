using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MCBBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MigrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Trigger the migration process
        [HttpPost("migrate")]
        public IActionResult MigrateData()
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("MigrateBCMOrderData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { message = "Migration completed successfully" });
        }

        // Fetch migrated data
        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT o.OrderID, s.Name AS SupplierName, a.Town, a.Village, o.OrderDate, o.Amount, o.Status FROM Orders o JOIN Suppliers s ON o.SupplierID = s.SupplierID JOIN Addresses a ON s.AddressID = a.AddressID", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return Ok(dt);
        }
    }
}
