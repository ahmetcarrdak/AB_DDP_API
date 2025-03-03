using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.DTO;

namespace DDPApi.Interfaces
{
    public interface IOrder
    {
        // Tüm siparişleri getirir
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

        // Aktif siparişleri getirir
        Task<IEnumerable<OrderDto>> GetActiveOrdersAsync();

        // ID'ye göre sipariş getirir
        Task<OrderDto> GetOrderByIdAsync(int id);

        // Yeni sipariş ekler
        Task<bool> AddOrderAsync(OrderDto orderDto);

        // Sipariş bilgilerini günceller
        Task<bool> UpdateOrderAsync(OrderDto orderDto);

        // Sipariş siler
        Task<bool> DeleteOrderAsync(int id);

        // Belirli bir müşterinin siparişlerini getirir
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId);
        
        // Belirli bir ürünün istasyon bilgilerini getirir
        Task<List<OrderStationDto>> GetOrderStationAsync();

        // Belirli bir tarih aralığındaki siparişleri getirir
        Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Belirli bir duruma sahip siparişleri getirir (Beklemede, Üretimde, Tamamlandı vb.)
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(int status);

        // Tüm siparişlerin durumlarını ve adetlerini getirir
        Task<IEnumerable<object>> GetOrderStatusesAsync();

        // Ödenmemiş siparişleri getirir
        Task<IEnumerable<OrderDto>> GetUnpaidOrdersAsync();

        // Belirli bir personele atanmış siparişleri getirir
        Task<IEnumerable<OrderDto>> GetOrdersByEmployeeIdAsync(int employeeId);

        // Öncelik durumuna göre siparişleri getirir
        Task<IEnumerable<OrderDto>> GetOrdersByPriorityAsync(string priority);

        // Sipariş kaynağına göre siparişleri getirir
        Task<IEnumerable<OrderDto>> GetOrdersBySourceAsync(string source);

        // Fatura numarasına göre sipariş getirir
        Task<OrderDto> GetOrderByInvoiceNumberAsync(string invoiceNumber);

        // Teslimatı geciken siparişleri getirir
        Task<IEnumerable<OrderDto>> GetDelayedOrdersAsync();

        // İptal edilen siparişleri getirir
        Task<IEnumerable<OrderDto>> GetCancelledOrdersAsync();

        // id'ye göre sipariş durumunu günceller 
        Task<bool> UpdateOrderStatusAsync(int orderId);
    }
}
