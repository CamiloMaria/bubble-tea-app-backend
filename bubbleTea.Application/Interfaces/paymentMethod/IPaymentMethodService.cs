using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<Response<IEnumerable<PaymentMethod>>> GetAllPaymentMethod(int page, int pageSize);
        Task<Response<PaymentMethod>> GetPaymentMethodById(int id);
        Task<Response<PaymentMethod>> CreatePaymentMethod(PaymentMethod paymentMethod);
        Task<Response<PaymentMethod>> UpdatePaymentMethod(PaymentMethod paymentMethod);
        Task<Response<PaymentMethod>> DeletePaymentMethod(int id);
        Task<Response<PaymentMethod>> GetPaymentMethodByOrderId(int orderId);
    }
}