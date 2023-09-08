using Microsoft.AspNetCore.Mvc;
using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BubbleTea.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) => _productService = productService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProduct(int page, int pageSize)
        {
            var response = new Response<IEnumerable<Product>>();

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
                var products = await _productService.GetAllProduct(page, pageSize);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = products.Data;
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
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = new Response<Product>();

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
                var product = await _productService.GetProductById(id);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = product.Data;
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
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var response = new Response<Product>();

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
                await _productService.CreateProduct(product);
                response.Success = true;
                response.StatusCode = 201;
                response.ReasonPhrase = "Created";
                response.Message = "Create user successfully!";
                response.Data = product;
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
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var response = new Response<Product>();

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
                await _productService.UpdateProduct(product);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Message = "Update user successfully!";
                response.Data = product;
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = new Response<Product>();

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
                await _productService.DeleteProduct(id);
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

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(int categoryId, int page, int pageSize)
        {
            var response = new Response<IEnumerable<Product>>();

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
                var products = await _productService.GetProductByCategory(categoryId, page, pageSize);
                response.Success = true;
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";
                response.Data = products.Data;
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
    }
}