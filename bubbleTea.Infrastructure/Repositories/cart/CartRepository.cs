using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.Storage;

namespace BubbleTea.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "cart";
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<CartRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Cart>>> GetAllCartAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var totalCarts = cacheEntry?.Count ?? 0;
                var pagedCarts = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedCarts)
                {
                    case null:
                        var message = "No se encontraron carritos";
                        var response = new Response<IEnumerable<Cart>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Cart>>
                        {
                            Data = pagedCarts,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalCarts,
                            TotalPages = (int)Math.Ceiling((double)totalCarts / pageSize),
                            Success = true,
                            Message = "Se encontraron carritos",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los carritos");
                return new Response<IEnumerable<Cart>>
                {
                    Success = false,
                    Message = "Error al obtener los carritos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> GetCartByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var cart = cacheEntry?.FirstOrDefault(c => c.Id == id);

                switch (cart)
                {
                    case null:
                        var message = $"No se encontró el carrito con el id {id}";
                        var response = new Response<Cart>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Cart>
                        {
                            Data = cart,
                            Success = true,
                            Message = $"Se encontró el carrito con el id {id}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el carrito con el id {id}", id);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al obtener el carrito con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> CreateCartAsync(Cart cart)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var existingCart = cacheEntry?.FirstOrDefault(c => c.Id == cart.Id);

                switch (existingCart)
                {
                    case { Id: var id } when id == cart.Id:
                        var message = $"Ya existe un carrito con el id {cart.Id}";
                        var response = new Response<Cart>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Carts.AddAsync(cart);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Cart>
                        {
                            Data = cart,
                            Success = true,
                            Message = $"Se creó el carrito con el id {cart.Id}",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el carrito con el id {cart.Id}", cart.Id);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al crear el carrito con el id {cart.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> UpdateCartAsync(Cart cart)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var existingCart = cacheEntry?.FirstOrDefault(c => c.Id == cart.Id);

                switch (existingCart)
                {
                    case null:
                        var message = $"No se encontró el carrito con el id {cart.Id}";
                        var response = new Response<Cart>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Carts.Entry(existingCart).State = EntityState.Detached;
                        _dbContext.Carts.Entry(cart).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Cart>
                        {
                            Data = existingCart,
                            Success = true,
                            Message = $"Se actualizó el carrito con el id {cart.Id}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el carrito con el id {cart.Id}", cart.Id);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al actualizar el carrito con el id {cart.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> DeleteCartAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var existingCart = cacheEntry?.FirstOrDefault(c => c.Id == id);

                switch (existingCart)
                {
                    case null:
                        var message = $"No se encontró el carrito con el id {id}";
                        var response = new Response<Cart>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Carts.Remove(existingCart);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Cart>
                        {
                            Data = existingCart,
                            Success = true,
                            Message = $"Se eliminó el carrito con el id {id}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el carrito con el id {id}", id);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al eliminar el carrito con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> GetCartByUserIdAsync(int userId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var cart = cacheEntry?.FirstOrDefault(c => c.UserId == userId);

                switch (cart)
                {
                    case null:
                        var message = $"No se encontró el carrito con el id de usuario {userId}";
                        var response = new Response<Cart>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Cart>
                        {
                            Data = cart,
                            Success = true,
                            Message = $"Se encontró el carrito con el id de usuario {userId}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el carrito con el id de usuario {userId}", userId);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al obtener el carrito con el id de usuario {userId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Cart>> GetCartByProductIdAsync(int productId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Carts.ToListAsync();
                });

                var cart = cacheEntry?.FirstOrDefault(c => c.ProductId == productId);

                switch (cart)
                {
                    case null:
                        var message = $"No se encontró el carrito con el id de producto {productId}";
                        var response = new Response<Cart>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Cart>
                        {
                            Data = cart,
                            Success = true,
                            Message = $"Se encontró el carrito con el id de producto {productId}",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el carrito con el id de producto {productId}", productId);
                return new Response<Cart>
                {
                    Success = false,
                    Message = $"Error al obtener el carrito con el id de producto {productId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}