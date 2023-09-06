using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class ProductToppingRepository : IProductToppingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "productToppings";
        private readonly ILogger<ProductToppingRepository> _logger;

        public ProductToppingRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<ProductToppingRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }
        public async Task<Response<IEnumerable<ProductTopping>>> GetAllProductToppingAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var totalProductTopping = cacheEntry?.Count ?? 0;
                var pagedProductTopping = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedProductTopping)
                {
                    case null:
                        var message = "No se encontraron productos con topping";
                        var response = new Response<IEnumerable<ProductTopping>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                return new Response<IEnumerable<ProductTopping>>
                {
                    Data = pagedProductTopping,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalProductTopping,
                    TotalPages = (int)Math.Ceiling((double)totalProductTopping / pageSize),
                    Success = true,
                    Message = "Se obtuvo satisfactoriamente los productos con topping",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos con topping");
                return new Response<IEnumerable<ProductTopping>>
                {
                    Success = false,
                    Message = "Error al obtener los productos con topping",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> GetProductToppingByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var productTopping = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (productTopping)
                {
                    case null:
                        var message = $"No se encontró el producto con topping con id {id}";
                        var response = new Response<ProductTopping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se obtuvo satisfactoriamente el producto con topping con id {id}",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con topping con id {id}", id);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al obtener el producto con topping con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> CreateProductToppingAsync(ProductTopping productTopping)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var existingProductTopping = cacheEntry?.FirstOrDefault(x => x.ProductId == productTopping.ProductId && x.ToppingId == productTopping.ToppingId);

                switch (existingProductTopping)
                {
                    case { Id: var id } when id == productTopping.Id:
                        var message = $"Ya existe un producto con topping con el id {id}";
                        var response = new Response<ProductTopping>(message, 400, "BadRequest");
                        response.AddError(message);
                        return response;
                    // case ya existe un producto con topping con el mismo producto y topping
                    case { ProductId: var productId, ToppingId: var toppingId } when productId == productTopping.ProductId && toppingId == productTopping.ToppingId:
                        message = $"Ya existe un producto con topping con el producto {productId} y topping {productTopping.ToppingId}";
                        response = new Response<ProductTopping>(message, 400, "BadRequest");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                await _dbContext.ProductToppings.AddAsync(productTopping);
                await _dbContext.SaveChangesAsync();

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se creó satisfactoriamente el producto con topping con id {productTopping.Id}",
                    StatusCode = 201,
                    ReasonPhrase = "Created"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto con topping con id {productTopping.Id}", productTopping.Id);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al crear el producto con topping con id {productTopping.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> UpdateProductToppingAsync(ProductTopping productTopping)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var existingProductTopping = cacheEntry?.FirstOrDefault(x => x.Id == productTopping.Id);

                switch (existingProductTopping)
                {
                    case null:
                        var message = $"No se encontró el producto con topping con id {productTopping.Id}";
                        var response = new Response<ProductTopping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { ProductId: var productId, ToppingId: var toppingId } when productId == productTopping.ProductId && toppingId == productTopping.ToppingId:
                        message = $"Ya existe un producto con topping con el producto {productId} y topping {productTopping.ToppingId}";
                        response = new Response<ProductTopping>(message, 400, "BadRequest");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                _dbContext.Entry(existingProductTopping).State = EntityState.Detached;
                _dbContext.Entry(productTopping).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se actualizó satisfactoriamente el producto con topping con id {productTopping.Id}",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con topping con id {productTopping.Id}", productTopping.Id);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al actualizar el producto con topping con id {productTopping.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> DeleteProductToppingAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var productTopping = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (productTopping)
                {
                    case null:
                        var message = $"No se encontró el producto con topping con id {id}";
                        var response = new Response<ProductTopping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                _dbContext.ProductToppings.Remove(productTopping);
                await _dbContext.SaveChangesAsync();

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se eliminó satisfactoriamente el producto con topping con id {id}",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto con topping con id {id}", id);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al eliminar el producto con topping con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> GetProductToppingByProductIdAsync(int productId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var productTopping = cacheEntry?.FirstOrDefault(x => x.ProductId == productId);

                switch (productTopping)
                {
                    case null:
                        var message = $"No se encontró el producto con topping con producto id {productId}";
                        var response = new Response<ProductTopping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se obtuvo satisfactoriamente el producto con topping con producto id {productId}",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con topping con producto id {productId}", productId);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al obtener el producto con topping con producto id {productId}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }

        public async Task<Response<ProductTopping>> GetProductToppingByToppingIdAsync(int toppingId)
        {
            try
            {

                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductToppings.ToListAsync();
                });

                var productTopping = cacheEntry?.FirstOrDefault(x => x.ToppingId == toppingId);

                switch (productTopping)
                {
                    case null:
                        var message = $"No se encontró el producto con topping con topping id {toppingId}";
                        var response = new Response<ProductTopping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { }:
                        break;
                }

                return new Response<ProductTopping>
                {
                    Data = productTopping,
                    Message = $"Se obtuvo satisfactoriamente el producto con topping con topping id {toppingId}",
                    StatusCode = 200,
                    ReasonPhrase = "Ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con topping con topping id {toppingId}", toppingId);
                return new Response<ProductTopping>
                {
                    Success = false,
                    Message = $"Error al obtener el producto con topping con topping id {toppingId}",
                    StatusCode = 500,
                    ReasonPhrase = "InternalServerError"
                };
            }
        }
    }
}
