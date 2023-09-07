using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "ProductSizes";
        private readonly ILogger<ProductSizeRepository> _logger;

        public ProductSizeRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<ProductSizeRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetAllProductSizeAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var totalSize = cacheEntry?.Count ?? 0;
                var pagedSize = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedSize)
                {
                    case null:
                        var message = "No se encontraron tamaños de productos";
                        var response = new Response<IEnumerable<ProductSize>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<ProductSize>>
                        {
                            Data = pagedSize,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalSize,
                            TotalPages = (int)Math.Ceiling((double)totalSize / pageSize),
                            Success = true,
                            Message = "Tamaños de productos encontrados",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tamaños de productos");
                return new Response<IEnumerable<ProductSize>>
                {
                    Success = false,
                    Message = "Error al obtener los tamaños de productos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<ProductSize>> GetProductSizeByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var productSize = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (productSize)
                {
                    case null:
                        var message = $"No se encontró el tamaño de producto con id {id}";
                        var response = new Response<ProductSize>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<ProductSize>
                        {
                            Data = productSize,
                            Success = true,
                            Message = $"Tamaño de producto con id {id} encontrado",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tamaño de producto con id {id}", id);
                return new Response<ProductSize>
                {
                    Success = false,
                    Message = $"Error al obtener el tamaño de producto con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<ProductSize>> CreateProductSizeAsync(ProductSize productSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var existingProductSize = cacheEntry?.FirstOrDefault(x => x.ProductId == productSize.ProductId && x.SizeId == productSize.SizeId);

                switch (existingProductSize)
                {
                    case { Id: var id} when id == productSize.Id:
                        var message = $"Ya existe un tamaño de producto con id {id}";
                        var response = new Response<ProductSize>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    // case ya existe un tamaño de producto con el mismo producto y tamaño
                    case { ProductId: var productId, SizeId: var sizeId } when productId == productSize.ProductId && sizeId == productSize.SizeId:
                        message = $"Ya existe un tamaño de producto con producto id {productId} y tamaño id {sizeId}";
                        response = new Response<ProductSize>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.ProductSizes.AddAsync(productSize);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<ProductSize>
                        {
                            Data = productSize,
                            Success = true,
                            Message = $"Se creo el tamaño de producto con id {productSize.Id}",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el tamaño de producto con id {id}", productSize.Id);
                return new Response<ProductSize>
                {
                    Success = false,
                    Message = $"Error al crear el tamaño de producto con id {productSize.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<ProductSize>> UpdateProductSizeAsync(ProductSize productSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var existingProductSize = cacheEntry?.FirstOrDefault(x => x.Id == productSize.Id);

                switch (existingProductSize)
                {
                    case null:
                        var message = $"No se encontró el tamaño de producto con id {productSize.Id}";
                        var response = new Response<ProductSize>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { ProductId: var productId, SizeId: var sizeId } when productId == productSize.ProductId && sizeId == productSize.SizeId:
                        message = $"Ya existe un tamaño de producto con producto id {productId} y tamaño id {sizeId}";
                        response = new Response<ProductSize>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.ProductSizes.Entry(existingProductSize).State = EntityState.Detached;
                        _dbContext.ProductSizes.Entry(productSize).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<ProductSize>
                        {
                            Data = productSize,
                            Success = true,
                            Message = $"Se actualizó el tamaño de producto con id {productSize.Id}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tamaño de producto con id {id}", productSize.Id);
                return new Response<ProductSize>
                {
                    Success = false,
                    Message = $"Error al actualizar el tamaño de producto con id {productSize.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<ProductSize>> DeleteProductSizeAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var productSize = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (productSize)
                {
                    case null:
                        var message = $"No se encontró el tamaño de producto con id {id}";
                        var response = new Response<ProductSize>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.ProductSizes.Remove(productSize);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<ProductSize>
                        {
                            Data = productSize,
                            Success = true,
                            Message = $"Se eliminó el tamaño de producto con id {id}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tamaño de producto con id {id}", id);
                return new Response<ProductSize>
                {
                    Success = false,
                    Message = $"Error al eliminar el tamaño de producto con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetProductSizeByProductIdAsync(int productId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var productSizes = cacheEntry?.Where(x => x.ProductId == productId);

                switch (productSizes)
                {
                    case null:
                        var message = $"No se encontraron tamaños de producto con producto id {productId}";
                        var response = new Response<IEnumerable<ProductSize>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<ProductSize>>
                        {
                            Data = productSizes,
                            Success = true,
                            Message = $"Tamaños de producto con producto id {productId} encontrados",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tamaños de producto con producto id {productId}", productId);
                return new Response<IEnumerable<ProductSize>>
                {
                    Success = false,
                    Message = $"Error al obtener los tamaños de producto con producto id {productId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetProductSizeBySizeIdAsync(int sizeId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.ProductSizes.ToListAsync();
                });

                var productSizes = cacheEntry?.Where(x => x.SizeId == sizeId);

                switch (productSizes)
                {
                    case null:
                        var message = $"No se encontraron tamaños de producto con tamaño id {sizeId}";
                        var response = new Response<IEnumerable<ProductSize>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<ProductSize>>
                        {
                            Data = productSizes,
                            Success = true,
                            Message = $"Tamaños de producto con tamaño id {sizeId} encontrados",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tamaños de producto con tamaño id {sizeId}", sizeId);
                return new Response<IEnumerable<ProductSize>>
                {
                    Success = false,
                    Message = $"Error al obtener los tamaños de producto con tamaño id {sizeId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}