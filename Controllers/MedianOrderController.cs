using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;

namespace MCBBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedianOrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MedianOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/MedianOrder
        [HttpGet]
        public IActionResult GetMedianOrder()
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetMedianOrderRecord", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                // Convert the single row into an anonymous object
                var row = dt.Rows[0];
                var result = new
                {
                    OrderReference = row["OrderReference"],
                    OrderDate = row["OrderDate"],
                    SupplierName = row["SupplierName"],
                    OrderTotalAmount = row["OrderTotalAmount"],
                    OrderStatus = row["OrderStatus"],
                    InvoiceReferences = row["InvoiceReferences"]
                };

                return Ok(result);
            }
            else
            {
                return NotFound("No order data found.");
            }
        }
    }
}
