using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;
using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                DeliveryAddress = order.DeliveryAddress,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                OrderStatus = order.OrderStatus,
                EstimatedDeliveryDate = order.EstimatedDeliveryDate,
                ActualDeliveryDate = order.ActualDeliveryDate,
                PaymentMethod = order.PaymentMethod,
                IsPaid = order.IsPaid,
                PaymentStatus = order.PaymentStatus,
                AssignedEmployeeId = order.AssignedEmployeeId,
                SpecialInstructions = order.SpecialInstructions,
                Priority = order.Priority,
                IsActive = order.IsActive,
                CancellationReason = order.CancellationReason,
                CancellationDate = order.CancellationDate,
                OrderSource = order.OrderSource,
                DiscountAmount = order.DiscountAmount,
                TaxAmount = order.TaxAmount,
                InvoiceNumber = order.InvoiceNumber
            };
        }

        private Order MapToEntity(OrderDto orderDto)
        {
            return new Order
            {
                OrderId = orderDto.OrderId,
                OrderDate = orderDto.OrderDate,
                CustomerName = orderDto.CustomerName,
                DeliveryAddress = orderDto.DeliveryAddress,
                CustomerPhone = orderDto.CustomerPhone,
                CustomerEmail = orderDto.CustomerEmail,
                ProductName = orderDto.ProductName,
                Quantity = orderDto.Quantity,
                UnitPrice = orderDto.UnitPrice,
                OrderStatus = orderDto.OrderStatus,
                EstimatedDeliveryDate = orderDto.EstimatedDeliveryDate,
                ActualDeliveryDate = orderDto.ActualDeliveryDate,
                PaymentMethod = orderDto.PaymentMethod,
                IsPaid = orderDto.IsPaid,
                PaymentStatus = orderDto.PaymentStatus,
                AssignedEmployeeId = orderDto.AssignedEmployeeId,
                SpecialInstructions = orderDto.SpecialInstructions,
                Priority = orderDto.Priority,
                IsActive = orderDto.IsActive,
                CancellationReason = orderDto.CancellationReason,
                CancellationDate = orderDto.CancellationDate,
                OrderSource = orderDto.OrderSource,
                DiscountAmount = orderDto.DiscountAmount,
                TaxAmount = orderDto.TaxAmount,
                InvoiceNumber = orderDto.InvoiceNumber,
            };
        }

        // Tüm siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(MapToDto);
        }

        // Aktif siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetActiveOrdersAsync()
        {
            var orders = await _context.Orders.Where(o => o.IsActive).ToListAsync();
            return orders.Select(MapToDto);
        }

        // ID'ye göre sipariş getirir
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order == null ? null : MapToDto(order);
        }

        // Yeni sipariş ekler
        public async Task<bool> AddOrderAsync(OrderDto orderDto)
        {
            try
            {
                var order = MapToEntity(orderDto);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sipariş bilgilerini günceller
        public async Task<bool> UpdateOrderAsync(OrderDto orderDto)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(orderDto.OrderId);
                if (existingOrder == null) return false;

                var updatedOrder = MapToEntity(orderDto);
                _context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sipariş siler
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Belirli bir müşterinin siparişlerini getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _context.Orders.Where(o => o.OrderId == customerId).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Belirli bir siparişin istasyon bilgilerini getirir
        public async Task<OrderStationDto> GetOrderStationAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new OrderStationDto
                {
                    StationId = o.StationId,
                    StagesId = o.StagesId,
                    OrderId = o.OrderId,
                    ProductName = o.ProductName,
                    CustomerName = o.CustomerName,
                    SpecialInstructions = o.SpecialInstructions,
                    Priority = o.Priority,
                    AssignedEmployeeId = o.AssignedEmployeeId,
                    Quantity = o.Quantity,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate
                })
                .FirstOrDefaultAsync();

            return order;
        }
        
        // Belirli bir tarih aralığındaki siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
            return orders.Select(MapToDto);
        }

        // Belirli bir duruma sahip siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _context.Orders.Where(o => o.OrderStatus == status).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Ödenmemiş siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetUnpaidOrdersAsync()
        {
            var orders = await _context.Orders.Where(o => !o.IsPaid).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Belirli bir personele atanmış siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersByEmployeeIdAsync(int employeeId)
        {
            var orders = await _context.Orders.Where(o => o.AssignedEmployeeId == employeeId).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Öncelik durumuna göre siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersByPriorityAsync(string priority)
        {
            var orders = await _context.Orders.Where(o => o.Priority == priority).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Sipariş kaynağına göre siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetOrdersBySourceAsync(string source)
        {
            var orders = await _context.Orders.Where(o => o.OrderSource == source).ToListAsync();
            return orders.Select(MapToDto);
        }

        // Fatura numarasına göre sipariş getirir
        public async Task<OrderDto> GetOrderByInvoiceNumberAsync(string invoiceNumber)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.InvoiceNumber == invoiceNumber);
            return order == null ? null : MapToDto(order);
        }

        // Teslimatı geciken siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetDelayedOrdersAsync()
        {
            var currentDate = DateTime.Now;
            var orders = await _context.Orders
                .Where(o => o.EstimatedDeliveryDate < currentDate && o.ActualDeliveryDate == null)
                .ToListAsync();
            return orders.Select(MapToDto);
        }

        // İptal edilen siparişleri getirir
        public async Task<IEnumerable<OrderDto>> GetCancelledOrdersAsync()
        {
            var orders = await _context.Orders.Where(o => !string.IsNullOrEmpty(o.CancellationReason)).ToListAsync();
            return orders.Select(MapToDto);
        }
    }
}
