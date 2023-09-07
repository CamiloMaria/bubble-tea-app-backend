using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "orderItems";
        private readonly ILogger<OrderItemRepository> _logger;

        public OrderItemRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<OrderItemRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<OrderItem>>> GetAllOrderItemAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var totalItems = cacheEntry?.Count ?? 0;
                var pagedItems = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedItems)
                {
                    case null:
                        var message = "No se encontraron datos";
                        var response = new Response<IEnumerable<OrderItem>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<OrderItem>>
                        {
                            Data = pagedItems,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalItems,
                            TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize),
                            Success = true,
                            Message = "Datos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos");
                return new Response<IEnumerable<OrderItem>>
                {
                    Success = false,
                    Message = "Error al obtener los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<OrderItem>> GetOrderItemByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var orderItem = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (orderItem)
                {
                    case null:
                        var message = $"No se encontró el registro con el id {id}";
                        var response = new Response<OrderItem>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<OrderItem>
                        {
                            Data = orderItem,
                            Success = true,
                            Message = "Datos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos");
                return new Response<OrderItem>
                {
                    Success = false,
                    Message = "Error al obtener los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<OrderItem>> CreateOrderItemAsync(OrderItem orderItem)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var existingOrderItem = cacheEntry?.FirstOrDefault(x => x.Id == orderItem.Id);

                switch (existingOrderItem)
                {
                    case { Id: var id } when id == orderItem.Id:
                        var message = $"Ya existe un registro con el id {id}";
                        var response = new Response<OrderItem>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.OrderItems.AddAsync(orderItem);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<OrderItem>
                        {
                            Data = orderItem,
                            Success = true,
                            Message = "Datos creados correctamente",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear los datos");
                return new Response<OrderItem>
                {
                    Success = false,
                    Message = "Error al crear los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<OrderItem>> UpdateOrderItemAsync(OrderItem orderItem)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var existingOrderItem = cacheEntry?.FirstOrDefault(x => x.Id == orderItem.Id);

                switch (existingOrderItem)
                {
                    case null:
                        var message = $"No se encontró el registro con el id {orderItem.Id}";
                        var response = new Response<OrderItem>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingOrderItem).State = EntityState.Detached;
                        _dbContext.Entry(orderItem).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<OrderItem>
                        {
                            Data = orderItem,
                            Success = true,
                            Message = "Datos actualizados correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los datos");
                return new Response<OrderItem>
                {
                    Success = false,
                    Message = "Error al actualizar los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<OrderItem>> DeleteOrderItemAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var orderItem = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (orderItem)
                {
                    case null:
                        var message = $"No se encontró el registro con el id {id}";
                        var response = new Response<OrderItem>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.OrderItems.Remove(orderItem);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<OrderItem>
                        {
                            Data = orderItem,
                            Success = true,
                            Message = "Datos eliminados correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar los datos");
                return new Response<OrderItem>
                {
                    Success = false,
                    Message = "Error al eliminar los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<OrderItem>>> GetOrderItemByOrderIdAsync(int orderId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var orderItems = cacheEntry?.Where(x => x.OrderId == orderId).ToList();

                switch (orderItems)
                {
                    case null:
                        var message = $"No se encontraron registros con el id {orderId}";
                        var response = new Response<IEnumerable<OrderItem>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<OrderItem>>
                        {
                            Data = orderItems,
                            Success = true,
                            Message = "Datos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos");
                return new Response<IEnumerable<OrderItem>>
                {
                    Success = false,
                    Message = "Error al obtener los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<OrderItem>>> GetOrderItemByProductIdAsync(int productId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.OrderItems.ToListAsync();
                });

                var orderItems = cacheEntry?.Where(x => x.ProductId == productId).ToList();

                switch (orderItems)
                {
                    case null:
                        var message = $"No se encontraron registros con el id {productId}";
                        var response = new Response<IEnumerable<OrderItem>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<OrderItem>>
                        {
                            Data = orderItems,
                            Success = true,
                            Message = "Datos obtenidos correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos");
                return new Response<IEnumerable<OrderItem>>
                {
                    Success = false,
                    Message = "Error al obtener los datos",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}