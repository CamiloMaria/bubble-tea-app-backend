using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "orders";
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<OrderRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Order>>> GetAllOrderAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var totalOrders = cacheEntry?.Count ?? 0;
                var pagedOrders = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedOrders)
                {
                    case null:
                        var message = "No se encontraron ordenes";
                        var response = new Response<IEnumerable<Order>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Order>>
                        {
                            Data = pagedOrders,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalOrders,
                            TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize),
                            Success = true,
                            Message = "Ordenes obtenidas correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ordenes");
                return new Response<IEnumerable<Order>>
                {
                    Success = false,
                    Message = "Error al obtener las ordenes",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Order>> GetOrderByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var order = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (order)
                {
                    case null:
                        var message = $"No se encontro la orden con el id {id}";
                        var response = new Response<Order>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Order>
                        {
                            Data = order,
                            Success = true,
                            Message = "Orden obtenida correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la orden con el id {id}", id);
                return new Response<Order>
                {
                    Success = false,
                    Message = $"Error al obtener la orden con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Order>> CreateOrderAsync(Order order)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var existingOrder = cacheEntry?.FirstOrDefault(x => x.Id == order.Id);

                switch (existingOrder)
                {
                    case { Id: var id } when id == order.Id:
                        var message = $"Ya existe una orden con el id {order.Id}";
                        var response = new Response<Order>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Orders.AddAsync(order);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Order>
                        {
                            Data = order,
                            Success = true,
                            Message = "Orden creada correctamente",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                        
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la orden con el id {order.Id}", order.Id);
                return new Response<Order>
                {
                    Success = false,
                    Message = $"Error al crear la orden con el id {order.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Order>> UpdateOrderAsync(Order order)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var existingOrder = cacheEntry?.FirstOrDefault(x => x.Id == order.Id);

                switch (existingOrder)
                {
                    case null:
                        var message = $"No se encontro la orden con el id {order.Id}";
                        var response = new Response<Order>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingOrder).State = EntityState.Detached;
                        _dbContext.Entry(order).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Order>
                        {
                            Data = order,
                            Success = true,
                            Message = "Orden actualizada correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la orden con el id {order.Id}", order.Id);
                return new Response<Order>
                {
                    Success = false,
                    Message = $"Error al actualizar la orden con el id {order.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Order>> DeleteOrderAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var existingOrder = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingOrder)
                {
                    case null:
                        var message = $"No se encontro la orden con el id {id}";
                        var response = new Response<Order>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when existingOrder.OrderItems.Any():
                        message = $"No se puede eliminar la orden con el id {id} porque tiene items asociados";
                        response = new Response<Order>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Orders.Remove(existingOrder);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Order>
                        {
                            Data = existingOrder,
                            Success = true,
                            Message = "Orden eliminada correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la orden con el id {id}", id);
                return new Response<Order>
                {
                    Success = false,
                    Message = $"Error al eliminar la orden con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<Order>>> GetOrderByUserIdAsync(int userId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Orders.ToListAsync();
                });

                var orders = cacheEntry?.Where(x => x.UserId == userId).ToList();

                switch (orders)
                {
                    case null:
                        var message = $"No se encontraron ordenes para el usuario con el id {userId}";
                        var response = new Response<IEnumerable<Order>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Order>>
                        {
                            Data = orders,
                            Success = true,
                            Message = "Ordenes obtenidas correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ordenes para el usuario con el id {userId}", userId);
                return new Response<IEnumerable<Order>>
                {
                    Success = false,
                    Message = $"Error al obtener las ordenes para el usuario con el id {userId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}