using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "products";
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Product>>> GetAllProductAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var totalProducts = cacheEntry?.Count ?? 0;
                var pagedProducts = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedProducts)
                {
                    case null:
                        var message = $"No se encontraron productos";
                        var response = new Response<IEnumerable<Product>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Product>>
                        {
                            Data = pagedProducts,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalProducts,
                            TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                            Success = true,
                            Message = "Productos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "Ok"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos");
                return new Response<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "Error al obtener los productos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<Product>> GetProductByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var product = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (product)
                {
                    case null:
                        var message = $"No se encontró el producto con id {id}";
                        var response = new Response<Product>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Product>
                        {
                            Data = product,
                            Success = true,
                            Message = "Producto obtenido correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "Ok"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con id {id}", id);
                return new Response<Product>
                {
                    Success = false,
                    Message = "Error al obtener el producto",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<Product>> CreateProductAsync(Product product)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var existingProduct = cacheEntry?.FirstOrDefault(x => x.Name == product.Name);

                switch (existingProduct)
                {
                    case { Name: var name } when name == product.Name:
                        var message = $"Ya existe un producto con el nombre {product.Name}";
                        var response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                    await _dbContext.Products.AddAsync(product);
                    await _dbContext.SaveChangesAsync();

                    _cache.Remove(_cacheKey);

                    return new Response<Product>
                    {
                        Data = product,
                        Success = true,
                        Message = "Producto creado correctamente",
                        StatusCode = 201,
                        ReasonPhrase = "Created"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto {product}", product);
                return new Response<Product>
                {
                    Success = false,
                    Message = "Error al crear el producto",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<Product>> UpdateProductAsync(Product product)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var existingProduct = cacheEntry?.FirstOrDefault(x => x.Id == product.Id);

                switch (existingProduct)
                {
                    case null:
                        var message = $"No se encontró el producto con id {product.Id}";
                        var response = new Response<Product>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Name: var name } when name == product.Name:
                        message = $"Ya existe un producto con el nombre {product.Name}";
                        response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingProduct).State = EntityState.Detached;
                        _dbContext.Entry(product).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Product>
                        {
                            Data = product,
                            Success = true,
                            Message = "Producto actualizado correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "Ok"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con id {id}", product.Id);
                return new Response<Product>
                {
                    Success = false,
                    Message = "Error al actualizar el producto",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<Product>> DeleteProductAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var existingProduct = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingProduct)
                {
                    case null:
                        var message = $"No se encontró el producto con id {id}";
                        var response = new Response<Product>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when existingProduct.OrderItems.Any():
                        message = $"No se puede eliminar el producto con id {id} porque tiene ordenes asociadas";
                        response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { } when existingProduct.ProductSizes.Any():
                        message = $"No se puede eliminar el producto con id {id} porque tiene tamaños asociados";
                        response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { } when existingProduct.ProductToppings.Any():
                        message = $"No se puede eliminar el producto con id {id} porque tiene toppings asociados";
                        response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { } when existingProduct.Carts.Any():
                        message = $"No se puede eliminar el producto con id {id} porque tiene carritos asociados";
                        response = new Response<Product>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Products.Remove(existingProduct);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Product>
                        {
                            Data = existingProduct,
                            Success = true,
                            Message = "Producto eliminado correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "Ok"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto con id {id}", id);
                return new Response<Product>
                {
                    Success = false,
                    Message = "Error al eliminar el producto",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<IEnumerable<Product>>> GetProductByCategoryAsync(int categoryId, int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Products.ToListAsync();
                });

                var products = cacheEntry?.Where(x => x.CategoryId == categoryId);
                var totalProducts = products?.Count() ?? 0;
                var pagedProducts = products?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedProducts)
                {
                    case null:
                        var message = $"No se encontraron productos con la categoría {categoryId}";
                        var response = new Response<IEnumerable<Product>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Product>>
                        {
                            Data = pagedProducts,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalProducts,
                            TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                            Success = true,
                            Message = "Productos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "Ok"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos con la categoría {categoryId}", categoryId);
                return new Response<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "Error al obtener los productos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }
    }
}