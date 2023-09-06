using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class SizeRepository :ISizeRepository
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
                    entry.SlidingExpiration = TimeSpan.FromMinutes(5);
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
                    case { }:
                        break;
                }

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
    }
}