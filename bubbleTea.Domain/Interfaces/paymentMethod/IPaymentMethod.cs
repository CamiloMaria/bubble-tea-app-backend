using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<Response<IEnumerable<PaymentMethod>>> GetAllPaymentMethodAsync(int page, in int pageSize);
        Task<Response<PaymentMethod>> GetPaymentMethodByIdAsync(int id);
        Task<Response<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod);
        Task<Response<PaymentMethod>> UpdatePaymentMethodAsync(PaymentMethod paymentMethod);
        Task<Response<PaymentMethod>> DeletePaymentMethodAsync(int id);
        Task<Response<PaymentMethod>> GetPaymentMethodByOrderIdAsync(int orderId);
    }
}