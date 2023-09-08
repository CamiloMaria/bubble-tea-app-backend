using Microsoft.AspNetCore.Mvc;
using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BubbleTea.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService) => _orderItemService = orderItemService;

        [HttpGet("page={page}&pageSize={pageSize}")]
        public async Task<IActionResult> GetAllOrderItem(int page, int pageSize)
        {
            var response = new Response<IEnumerable<OrderItem>>();

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
                var orderItems = await _orderItemService.GetAllOrderItem(page, pageSize);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get all order item successfully!";
                response.Data = orderItems.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get all order item failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var response = new Response<OrderItem>();

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
                var orderItem = await _orderItemService.GetOrderItemById(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get order item by id successfully!";
                response.Data = orderItem.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get order item by id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(OrderItem orderItem)
        {
            var response = new Response<OrderItem>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order item data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var newOrderItem = await _orderItemService.CreateOrderItem(orderItem);
                response.Success = true;
                response.StatusCode = 201;
                response.ReasonPhrase = "Created";
                response.Message = "Create order item successfully!";
                response.Data = newOrderItem.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Create order item failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(OrderItem orderItem)
        {
            var response = new Response<OrderItem>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order item data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var updatedOrderItem = await _orderItemService.UpdateOrderItem(orderItem);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Update order item successfully!";
                response.Data = updatedOrderItem.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Update order item failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var response = new Response<OrderItem>();

            try
            {
                var deletedOrderItem = await _orderItemService.DeleteOrderItem(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Delete order item successfully!";
                response.Data = deletedOrderItem.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Delete order item failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderItemByOrderId(int orderId)
        {
            var response = new Response<IEnumerable<OrderItem>>();

            try
            {
                var orderItems = await _orderItemService.GetOrderItemByOrderId(orderId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get order item by order id successfully!";
                response.Data = orderItems.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get order item by order id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetOrderItemByProductId(int productId)
        {
            var response = new Response<IEnumerable<OrderItem>>();

            try
            {
                var orderItems = await _orderItemService.GetOrderItemByProductId(productId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get order item by product id successfully!";
                response.Data = orderItems.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get order item by product id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}