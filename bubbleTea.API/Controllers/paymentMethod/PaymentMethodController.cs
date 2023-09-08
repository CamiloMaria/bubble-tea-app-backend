using Microsoft.AspNetCore.Mvc;
using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BubbleTea.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService) => _paymentMethodService = paymentMethodService;

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentMethod(int page, int pageSize)
        {
            var response = new Response<IEnumerable<PaymentMethod>>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid user data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var paymentMethods = await _paymentMethodService.GetAllPaymentMethod();
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get all payment method successfully!";
                response.Data = paymentMethods.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get all user failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethodById(int id)
        {
            var response = new Response<PaymentMethod>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid user data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodById(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get payment method by id successfully!";
                response.Data = paymentMethod.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get user by id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            var response = new Response<PaymentMethod>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid user data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var newPaymentMethod = await _paymentMethodService.CreatePaymentMethod(paymentMethod);
                response.Success = true;
                response.StatusCode = 201;
                response.ReasonPhrase = "Created";
                response.Message = "Create user successfully!";
                response.Data = newPaymentMethod.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Create user failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] PaymentMethod paymentMethod)
        {
            var response = new Response<PaymentMethod>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid user data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                paymentMethod.Id = id;
                var paymentMethodUpdated = await _paymentMethodService.UpdatePaymentMethod(paymentMethod);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Update user successfully!";
                response.Data = paymentMethodUpdated.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Update user failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var response = new Response<PaymentMethod>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid user data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                await _paymentMethodService.DeletePaymentMethod(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Delete user successfully!";
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Delete user failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentMethodByOrderId(int orderId)
        {
            var response = new Response<IEnumerable<PaymentMethod>>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid payment method data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }
            try
            {
                var paymentMethods = await _paymentMethodService.GetPaymentMethodByOrderId(orderId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get payment method by order id successfully!";
                response.Data = paymentMethods.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = $"Get payment method by order id {orderId} failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}