using DDPApi.Data;
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

        // Tüm siparişleri getirir
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Aktif siparişleri getirir
        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            return await _context.Orders.Where(o => o.IsActive).ToListAsync();
        }

        // ID'ye göre sipariş getirir
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // Yeni sipariş ekler
        public async Task<bool> AddOrderAsync(Order order)
        {
            try
            {
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
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Modified;
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
        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders.Where(o => o.OrderId == customerId).ToListAsync();
        }

        // Belirli bir tarih aralığındaki siparişleri getirir
        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
        }

        // Belirli bir duruma sahip siparişleri getirir
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders.Where(o => o.OrderStatus == status).ToListAsync();
        }

        // Ödenmemiş siparişleri getirir
        public async Task<IEnumerable<Order>> GetUnpaidOrdersAsync()
        {
            return await _context.Orders.Where(o => !o.IsPaid).ToListAsync();
        }

        // Belirli bir personele atanmış siparişleri getirir
        public async Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int employeeId)
        {
            return await _context.Orders.Where(o => o.AssignedEmployeeId == employeeId).ToListAsync();
        }

        // Öncelik durumuna göre siparişleri getirir
        public async Task<IEnumerable<Order>> GetOrdersByPriorityAsync(string priority)
        {
            return await _context.Orders.Where(o => o.Priority == priority).ToListAsync();
        }

        // Sipariş kaynağına göre siparişleri getirir
        public async Task<IEnumerable<Order>> GetOrdersBySourceAsync(string source)
        {
            return await _context.Orders.Where(o => o.OrderSource == source).ToListAsync();
        }

        // Fatura numarasına göre sipariş getirir
        public async Task<Order> GetOrderByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.InvoiceNumber == invoiceNumber);
        }

        // Teslimatı geciken siparişleri getirir
        public async Task<IEnumerable<Order>> GetDelayedOrdersAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.Orders
                .Where(o => o.EstimatedDeliveryDate < currentDate && o.ActualDeliveryDate == null)
                .ToListAsync();
        }

        // İptal edilen siparişleri getirir
        public async Task<IEnumerable<Order>> GetCancelledOrdersAsync()
        {
            return await _context.Orders.Where(o => !string.IsNullOrEmpty(o.CancellationReason)).ToListAsync();
        }
    }
}
