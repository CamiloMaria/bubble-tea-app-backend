using Microsoft.AspNetCore.Mvc;
using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BubbleTea.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductToppingController : ControllerBase
    {
        private readonly IProductToppingService _productToppingService;
        public ProductToppingController(IProductToppingService productToppingService) => _productToppingService = productToppingService;

        [HttpGet]
        public async Task<IActionResult> GetAllProductTopping(int page, int pageSize)
        {
            var response = new Response<IEnumerable<ProductTopping>>();

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
                var productToppings = await _productToppingService.GetAllProductTopping(page, pageSize);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = productToppings.Data;
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
        public async Task<IActionResult> GetProductToppingById(int id)
        {
            var response = new Response<ProductTopping>();

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
                var productTopping = await _productToppingService.GetProductToppingById(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = productTopping.Data;
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
        public async Task<IActionResult> CreateProductTopping(ProductTopping productTopping)
        {
            var response = new Response<ProductTopping>();

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
                await _productToppingService.CreateProductTopping(productTopping);
                response.Success = true;
                response.StatusCode = 201;
                response.ReasonPhrase = "Created";
                response.Message = "Create user successfully!";
                response.Data = productTopping;
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
        public async Task<IActionResult> UpdateProductTopping(int id, ProductTopping productTopping)
        {
            var response = new Response<ProductTopping>();

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
                await _productToppingService.UpdateProductTopping(productTopping);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Update user successfully!";
                response.Data = productTopping;
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
        public async Task<IActionResult> DeleteProductTopping(int id)
        {
            var response = new Response<ProductTopping>();

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
                await _productToppingService.DeleteProductTopping(id);
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

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductToppingByProductId(int productId)
        {
            var response = new Response<ProductTopping>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid product topping data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var productTopping = await _productToppingService.GetProductToppingByProductId(productId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = productTopping.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("topping/{toppingId}")]
        public async Task<IActionResult> GetProductToppingByToppingId(int toppingId)
        {
            var response = new Response<ProductTopping>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.StatusCode = 400;
                response.ReasonPhrase = "Bad Request";
                response.Message = "Invalid product topping data";
                response.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(xx => xx.ErrorMessage)
                    .ToList();
                return BadRequest(response);
            }

            try
            {
                var productTopping = await _productToppingService.GetProductToppingByToppingId(toppingId);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = productTopping.Data;
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = 500;
                response.ReasonPhrase = "Internal Server Error";
                response.AddError(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}