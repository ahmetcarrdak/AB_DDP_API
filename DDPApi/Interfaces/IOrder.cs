using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IOrder
    {
        // Tüm siparişleri getirir
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        // Aktif siparişleri getirir
        Task<IEnumerable<Order>> GetActiveOrdersAsync();

        // ID'ye göre sipariş getirir
        Task<Order> GetOrderByIdAsync(int id);

        // Yeni sipariş ekler
        Task<bool> AddOrderAsync(Order order);

        // Sipariş bilgilerini günceller
        Task<bool> UpdateOrderAsync(Order order);

        // Sipariş siler
        Task<bool> DeleteOrderAsync(int id);

        // Belirli bir müşterinin siparişlerini getirir
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);

        // Belirli bir tarih aralığındaki siparişleri getirir
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Belirli bir duruma sahip siparişleri getirir (Beklemede, Üretimde, Tamamlandı vb.)
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);

        // Ödenmemiş siparişleri getirir
        Task<IEnumerable<Order>> GetUnpaidOrdersAsync();

        // Belirli bir personele atanmış siparişleri getirir
        Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int employeeId);

        // Öncelik durumuna göre siparişleri getirir
        Task<IEnumerable<Order>> GetOrdersByPriorityAsync(string priority);

        // Sipariş kaynağına göre siparişleri getirir
        Task<IEnumerable<Order>> GetOrdersBySourceAsync(string source);

        // Fatura numarasına göre sipariş getirir
        Task<Order> GetOrderByInvoiceNumberAsync(string invoiceNumber);

        // Teslimatı geciken siparişleri getirir
        Task<IEnumerable<Order>> GetDelayedOrdersAsync();

        // İptal edilen siparişleri getirir
        Task<IEnumerable<Order>> GetCancelledOrdersAsync();
    }
}
