using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class ToppingRepository : IToppingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "toppings";
        private readonly ILogger<ToppingRepository> _logger;

        public ToppingRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<ToppingRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Topping>>> GetAllToppingAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Toppings.ToListAsync();
                });

                var totalToppings = cacheEntry?.Count ?? 0;
                var pagedToppings = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedToppings)
                {
                    case null:
                        var message = $"No se encontraron toppings";
                        var response = new Response<IEnumerable<Topping>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Topping>>
                        {
                            Data = pagedToppings,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalToppings,
                            TotalPages = (int)Math.Ceiling((double)totalToppings / pageSize),
                            Success = true,
                            Message = "Se obtuvieron los toppings correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los toppings");
                return new Response<IEnumerable<Topping>>
                {
                    Success = false,
                    Message = "Error al obtener los toppings",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Topping>> GetToppingByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Toppings.ToListAsync();
                });

                var topping = cacheEntry?.FirstOrDefault(t => t.Id == id);

                switch (topping)
                {
                    case null:
                        var message = $"No se encontró el topping con id {id}";
                        var response = new Response<Topping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Topping>
                        {
                            Data = topping,
                            Success = true,
                            Message = $"Se obtuvo el topping con id {id} correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el topping con id {id}", id);
                return new Response<Topping>
                {
                    Success = false,
                    Message = $"Error al obtener el topping con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Topping>> CreateToppingAsync(Topping topping)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Toppings.ToListAsync();
                });

                var existingTopping = cacheEntry?.FirstOrDefault(t => t.Name == topping.Name);

                switch (existingTopping)
                {
                    case { Name: var toppingName } when toppingName == topping.Name:
                        var message = $"Ya existe un topping con el nombre {topping.Name}";
                        var response = new Response<Topping>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Toppings.AddAsync(topping);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Topping>
                        {
                            Data = topping,
                            Success = true,
                            Message = "Topping creado correctamente",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el topping");
                return new Response<Topping>
                {
                    Success = false,
                    Message = "Error al crear el topping",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Topping>> UpdateToppingAsync(Topping topping)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Toppings.ToListAsync();
                });

                var existingTopping = await _dbContext.Toppings.FirstOrDefaultAsync(t => t.Id == topping.Id);

                switch (existingTopping)
                {
                    case null:
                        var message = $"No se encontró el topping con id {topping.Id}";
                        var response = new Response<Topping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Name: var toppingName } when toppingName == topping.Name:
                        message = $"Ya existe un topping con el nombre {topping.Name}";
                        response = new Response<Topping>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingTopping).State = EntityState.Detached;
                        _dbContext.Entry(topping).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Topping>
                        {
                            Data = existingTopping,
                            Success = true,
                            Message = "Topping actualizado correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el topping con id {topping.Id}", topping.Id);
                return new Response<Topping>
                {
                    Success = false,
                    Message = $"Error al actualizar el topping con id {topping.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Topping>> DeleteToppingAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Toppings.ToListAsync();
                });

                var existingTopping = await _dbContext.Toppings.FirstOrDefaultAsync(t => t.Id == id);

                switch (existingTopping)
                {
                    case null:
                        var message = $"No se encontró el topping con id {id}";
                        var response = new Response<Topping>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when existingTopping.ProductToppings.Any():
                        message = $"No se puede eliminar el topping con id {id} porque tiene productos asociados";
                        response = new Response<Topping>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Toppings.Remove(existingTopping);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Topping>
                        {
                            Data = existingTopping,
                            Message = "Topping eliminado correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el topping con id {id}", id);
                return new Response<Topping>
                {
                    Success = false,
                    Message = $"Error al eliminar el topping con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}