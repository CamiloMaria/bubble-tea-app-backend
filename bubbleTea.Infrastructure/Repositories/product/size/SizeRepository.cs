using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class SizeRepository : ISizeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "size";
        private readonly ILogger<SizeRepository> _logger;

        public SizeRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<SizeRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Size>>> GetAllSizeAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Sizes.ToListAsync();
                });

                var totalSize = cacheEntry?.Count ?? 0;
                var pagedSize = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedSize)
                {
                    case null:
                        var message = "No se encontraron tamaños";
                        var response = new Response<IEnumerable<Size>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Size>>
                        {
                            Data = pagedSize,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalSize,
                            TotalPages = (int)Math.Ceiling((decimal)totalSize / pageSize),
                            Success = true,
                            Message = "Se obtuvieron los tamaños correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tamaños");
                return new Response<IEnumerable<Size>>
                {
                    Success = false,
                    Message = "Error al obtener los tamaños",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Size>> GetSizeByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Sizes.ToListAsync();
                });

                var size = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (size)
                {
                    case null:
                        var message = "No se encontró el tamaño";
                        var response = new Response<Size>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Size>
                        {
                            Data = size,
                            Success = true,
                            Message = "Se obtuvo el tamaño correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tamaño");
                return new Response<Size>
                {
                    Success = false,
                    Message = "Error al obtener el tamaño",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Size>> CreateSizeAsync(Size size)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Sizes.ToListAsync();
                });

                var existingSize = cacheEntry?.FirstOrDefault(x => x.Name == size.Name);

                switch (existingSize)
                {
                    case { Name: var name } when name == size.Name:
                        var message = "Ya existe un tamaño con ese nombre";
                        var response = new Response<Size>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Sizes.AddAsync(size);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Size>
                        {
                            Data = size,
                            Success = true,
                            Message = "Se creó el tamaño correctamente",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el tamaño");
                return new Response<Size>
                {
                    Success = false,
                    Message = "Error al crear el tamaño",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Size>> UpdateSizeAsync(Size size)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Sizes.ToListAsync();
                });

                var existingSize = cacheEntry?.FirstOrDefault(x => x.Id == size.Id);

                switch (existingSize)
                {
                    case null:
                        var message = "No se encontró el tamaño";
                        var response = new Response<Size>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Name: var name } when name == size.Name:
                        message = "Ya existe un tamaño con ese nombre";
                        response = new Response<Size>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Sizes.Entry(existingSize).State = EntityState.Detached;
                        _dbContext.Sizes.Entry(size).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Size>
                        {
                            Data = existingSize,
                            Success = true,
                            Message = "Se actualizó el tamaño correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tamaño");
                return new Response<Size>
                {
                    Success = false,
                    Message = "Error al actualizar el tamaño",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Size>> DeleteSizeAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Sizes.ToListAsync();
                });

                var size = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (size)
                {
                    case null:
                        var message = "No se encontró el tamaño";
                        var response = new Response<Size>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when size.ProductSizes.Any():
                        message = "No se puede eliminar el tamaño porque tiene productos asociados";
                        response = new Response<Size>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Sizes.Remove(size);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Size>
                        {
                            Data = size,
                            Success = true,
                            Message = "Se eliminó el tamaño correctamente",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tamaño");
                return new Response<Size>
                {
                    Success = false,
                    Message = "Error al eliminar el tamaño",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}