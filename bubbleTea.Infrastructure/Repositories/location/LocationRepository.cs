using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "locations";
        private ILogger<LocationRepository> _logger;

        public LocationRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<LocationRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Location>>> GetAllLocationAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var totalLocations = cacheEntry?.Count ?? 0;
                var pagedLocations = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedLocations)
                {
                    case null:
                        var message = "No se encontraron ubicaciones";
                        var response = new Response<IEnumerable<Location>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Location>>
                        {
                            Data = pagedLocations,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalLocations,
                            TotalPages = (int)Math.Ceiling((double)totalLocations / pageSize),
                            Success = true,
                            Message = "Ubicaciones encontradas",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ubicaciones");
                return new Response<IEnumerable<Location>>
                {
                    Success = false,
                    Message = "Error al obtener las ubicaciones",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Location>> GetLocationByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var location = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (location)
                {
                    case null:
                        var message = $"No se encontró la ubicación con el id {id}";
                        var response = new Response<Location>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Location>
                        {
                            Data = location,
                            Success = true,
                            Message = "Ubicación encontrada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la ubicación con el id {id}");
                return new Response<Location>
                {
                    Success = false,
                    Message = $"Error al obtener la ubicación con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Location>> CreateLocationAsync(Location location)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var existingLocation = cacheEntry?.FirstOrDefault(x => x.Name == location.Name);

                switch (existingLocation)
                {
                    case { Name: var name } when name == location.Name:
                        var message = $"Ya existe una ubicación con el nombre {location.Name}";
                        var response = new Response<Location>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Locations.AddAsync(location);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Location>
                        {
                            Data = location,
                            Success = true,
                            Message = "Ubicación creada",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        }; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la ubicación");
                return new Response<Location>
                {
                    Success = false,
                    Message = "Error al crear la ubicación",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Location>> UpdateLocationAsync(Location location)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var existingLocation = cacheEntry?.FirstOrDefault(x => x.Id == location.Id);

                switch (existingLocation)
                {
                    case null:
                        var message = $"No se encontró la ubicación con el id {location.Id}";
                        var response = new Response<Location>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Name: var name } when name == location.Name:
                        message = $"Ya existe una ubicación con el nombre {location.Name}";
                        response = new Response<Location>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingLocation).State = EntityState.Detached;
                        _dbContext.Entry(location).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Location>
                        {
                            Data = existingLocation,
                            Success = true,
                            Message = $"Ubicación con el id {location.Id} actualizada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la ubicación con el id {location.Id}");
                return new Response<Location>
                {
                    Success = false,
                    Message = $"Error al actualizar la ubicación con el id {location.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Location>> DeleteLocationAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var existingLocation = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingLocation)
                {
                    case null:
                        var message = $"No se encontró la ubicación con el id {id}";
                        var response = new Response<Location>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Locations.Remove(existingLocation);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Location>
                        {
                            Data = existingLocation,
                            Success = true,
                            Message = $"Ubicación con el id {id} eliminada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la ubicación con el id {id}");
                return new Response<Location>
                {
                    Success = false,
                    Message = $"Error al eliminar la ubicación con el id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Location>> GetLocationByUserIdAsync(int userId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Locations.ToListAsync();
                });

                var location = cacheEntry?.FirstOrDefault(x => x.UserId == userId);

                switch (location)
                {
                    case null:
                        var message = $"No se encontró la ubicación con el id de usuario {userId}";
                        var response = new Response<Location>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Location>
                        {
                            Data = location,
                            Success = true,
                            Message = "Ubicación encontrada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la ubicación con el id de usuario {userId}");
                return new Response<Location>
                {
                    Success = false,
                    Message = $"Error al obtener la ubicación con el id de usuario {userId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}