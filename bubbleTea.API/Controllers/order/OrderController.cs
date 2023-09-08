using Microsoft.AspNetCore.Mvc;
using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BubbleTea.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) => _orderService = orderService;

        [HttpGet]
        public async Task<IActionResult> GetAllOrder(int page, int pageSize)
        {
            var response = new Response<IEnumerable<Order>>();

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
                var orders = await _orderService.GetAllOrder(page, pageSize);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get all order successfully!";
                response.Data = orders.Data;
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
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = new Response<Order>();

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
                var order = await _orderService.GetOrderById(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get order by id successfully!";
                response.Data = order.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get order by id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var response = new Response<Order>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var newOrder = await _orderService.CreateOrder(order);
                response.Success = true;
                response.StatusCode = 201;
                response.ReasonPhrase = "Created";
                response.Message = "Create order successfully!";
                response.Data = newOrder.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Create order failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            var response = new Response<Order>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                order.Id = id;
                var updatedOrder = await _orderService.UpdateOrder(order);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Update order successfully!";
                response.Data = updatedOrder.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Update order failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = new Response<Order>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                await _orderService.DeleteOrder(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Delete order successfully!";
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Delete order failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrderByUserId(int userId)
        {
            var response = new Response<IEnumerable<Order>>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid order data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var orders = await _orderService.GetOrderByUserId(userId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Get order by user id successfully!";
                response.Data = orders.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.Message = "Get order by user id failed!";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}
