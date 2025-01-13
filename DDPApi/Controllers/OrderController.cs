using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;

        public OrderController(IOrder orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Order>>> GetActiveOrders()
        {
            var orders = await _orderService.GetActiveOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var result = await _orderService.AddOrderAsync(order);
            if (!result)
                return BadRequest();
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
                return BadRequest();

            var result = await _orderService.UpdateOrderAsync(order);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: api/Order/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // GET: api/Order/daterange
        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        // GET: api/Order/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // GET: api/Order/unpaid
        [HttpGet("unpaid")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUnpaidOrders()
        {
            var orders = await _orderService.GetUnpaidOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByEmployee(int employeeId)
        {
            var orders = await _orderService.GetOrdersByEmployeeIdAsync(employeeId);
            return Ok(orders);
        }

        // GET: api/Order/priority/{priority}
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByPriority(string priority)
        {
            var orders = await _orderService.GetOrdersByPriorityAsync(priority);
            return Ok(orders);
        }

        // GET: api/Order/source/{source}
        [HttpGet("source/{source}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersBySource(string source)
        {
            var orders = await _orderService.GetOrdersBySourceAsync(source);
            return Ok(orders);
        }

        // GET: api/Order/invoice/{invoiceNumber}
        [HttpGet("invoice/{invoiceNumber}")]
        public async Task<ActionResult<Order>> GetOrderByInvoiceNumber(string invoiceNumber)
        {
            var order = await _orderService.GetOrderByInvoiceNumberAsync(invoiceNumber);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // GET: api/Order/delayed
        [HttpGet("delayed")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDelayedOrders()
        {
            var orders = await _orderService.GetDelayedOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/cancelled
        [HttpGet("cancelled")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCancelledOrders()
        {
            var orders = await _orderService.GetCancelledOrdersAsync();
            return Ok(orders);
        }
    }
}
