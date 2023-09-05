using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            this._paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<Response<IEnumerable<PaymentMethod>>> GetAllPaymentMethod(int page, int pageSize)
        {
            return await _paymentMethodRepository.GetAllPaymentMethodAsync(page, pageSize);
        }

        public async Task<Response<PaymentMethod>> GetPaymentMethodById(int id)
        {
            return await _paymentMethodRepository.GetPaymentMethodByIdAsync(id);
        }

        public async Task<Response<PaymentMethod>> CreatePaymentMethod(PaymentMethod paymentMethod)
        {
            return await _paymentMethodRepository.CreatePaymentMethodAsync(paymentMethod);
        }

        public async Task<Response<PaymentMethod>> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            return await _paymentMethodRepository.UpdatePaymentMethodAsync(paymentMethod);
        }

        public async Task<Response<PaymentMethod>> DeletePaymentMethod(int id)
        {
            return await _paymentMethodRepository.DeletePaymentMethodAsync(id);
        }

        public async Task<Response<PaymentMethod>> GetPaymentMethodByOrderId(int orderId)
        {
            return await _paymentMethodRepository.GetPaymentMethodByOrderIdAsync(orderId);
        }
    }
}