using DDPApi.Interfaces;
using DDPApi.DTO;
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetActiveOrders()
        {
            var orders = await _orderService.GetActiveOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            var result = await _orderService.AddOrderAsync(orderDto);
            if (!result)
                return BadRequest();
            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            if (id != orderDto.OrderId)
                return BadRequest();

            var result = await _orderService.UpdateOrderAsync(orderDto);
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // GET: api/OrderStation/{orderId}
        [HttpGet("StationInfo/{orderId}")]
        public async Task<ActionResult<OrderStationDto>> GetOrderStation(int orderId)
        {
            var orderStation = await _orderService.GetOrderStationAsync(orderId);
            if (orderStation == null)
                return NotFound();

            return Ok(orderStation);
        }
        
        // GET: api/Order/daterange
        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        // GET: api/Order/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // GET: api/Order/unpaid
        [HttpGet("unpaid")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUnpaidOrders()
        {
            var orders = await _orderService.GetUnpaidOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByEmployee(int employeeId)
        {
            var orders = await _orderService.GetOrdersByEmployeeIdAsync(employeeId);
            return Ok(orders);
        }

        // GET: api/Order/priority/{priority}
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByPriority(string priority)
        {
            var orders = await _orderService.GetOrdersByPriorityAsync(priority);
            return Ok(orders);
        }

        // GET: api/Order/source/{source}
        [HttpGet("source/{source}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersBySource(string source)
        {
            var orders = await _orderService.GetOrdersBySourceAsync(source);
            return Ok(orders);
        }

        // GET: api/Order/invoice/{invoiceNumber}
        [HttpGet("invoice/{invoiceNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrderByInvoiceNumber(string invoiceNumber)
        {
            var order = await _orderService.GetOrderByInvoiceNumberAsync(invoiceNumber);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // GET: api/Order/delayed
        [HttpGet("delayed")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetDelayedOrders()
        {
            var orders = await _orderService.GetDelayedOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/cancelled
        [HttpGet("cancelled")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCancelledOrders()
        {
            var orders = await _orderService.GetCancelledOrdersAsync();
            return Ok(orders);
        }
    }
}
