using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKey = "images";
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(ApplicationDbContext dbContext, IMemoryCache memoryCache, ILogger<ImageRepository> logger)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Image>>> GetAllImageAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var totalImages = cacheEntry?.Count ?? 0;
                var pagedImages = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedImages)
                {
                    case null:
                        var message = "No se encontraron imagenes";
                        var response = new Response<IEnumerable<Image>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Image>>
                        {
                            Data = pagedImages,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalImages,
                            TotalPages = (int)Math.Ceiling((double)totalImages / pageSize),
                            Success = true,
                            Message = "Imagenes encontradas",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las imagenes");
                return new Response<IEnumerable<Image>>
                {
                    Success = false,
                    Message = "Error al obtener las imagenes",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Image>> GetImageByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var image = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (image)
                {
                    case null:
                        var message = "No se encontro la imagen";
                        var response = new Response<Image>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Image>
                        {
                            Data = image,
                            Success = true,
                            Message = "Imagen encontrada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la imagen");
                return new Response<Image>
                {
                    Success = false,
                    Message = "Error al obtener la imagen",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Image>> CreateImageAsync(Image image)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var existingImage = cacheEntry?.FirstOrDefault(x => x.Id == image.Id);

                switch (existingImage)
                {
                    case { Id: var id } when id == image.Id:
                        var message = "La imagen ya existe";
                        var response = new Response<Image>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Images.AddAsync(image);
                        await _dbContext.SaveChangesAsync();

                        _memoryCache.Remove(_cacheKey);

                        return new Response<Image>
                        {
                            Data = image,
                            Success = true,
                            Message = "Imagen creada",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la imagen");
                return new Response<Image>
                {
                    Success = false,
                    Message = "Error al crear la imagen",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Image>> UpdateImageAsync(Image image)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var existingImage = cacheEntry?.FirstOrDefault(x => x.Id == image.Id);

                switch (existingImage)
                {
                    case null:
                        var message = "La imagen no existe";
                        var response = new Response<Image>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingImage).State = EntityState.Detached;
                        _dbContext.Entry(image).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _memoryCache.Remove(_cacheKey);

                        return new Response<Image>
                        {
                            Data = image,
                            Success = true,
                            Message = "Imagen actualizada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la imagen");
                return new Response<Image>
                {
                    Success = false,
                    Message = "Error al actualizar la imagen",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Image>> DeleteImageAsync(int id)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var existingImage = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingImage)
                {
                    case null:
                        var message = "La imagen no existe";
                        var response = new Response<Image>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Images.Remove(existingImage);
                        await _dbContext.SaveChangesAsync();

                        _memoryCache.Remove(_cacheKey);

                        return new Response<Image>
                        {
                            Data = existingImage,
                            Success = true,
                            Message = "Imagen eliminada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la imagen");
                return new Response<Image>
                {
                    Success = false,
                    Message = "Error al eliminar la imagen",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<Image>>> GetImageByProductIdAsync(int productId)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var images = cacheEntry?.Where(x => x.ProductId == productId).ToList();

                switch (images)
                {
                    case null:
                        var message = "No se encontraron imagenes";
                        var response = new Response<IEnumerable<Image>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Image>>
                        {
                            Data = images,
                            Success = true,
                            Message = "Imagenes encontradas",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las imagenes");
                return new Response<IEnumerable<Image>>
                {
                    Success = false,
                    Message = "Error al obtener las imagenes",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Image>> GetImageByUserIdAsync(int userId)
        {
            try
            {
                var cacheEntry = await _memoryCache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Images.ToListAsync();
                });

                var image = cacheEntry?.FirstOrDefault(x => x.UserId == userId);

                switch (image)
                {
                    case null:
                        var message = "No se encontro la imagen";
                        var response = new Response<Image>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Image>
                        {
                            Data = image,
                            Success = true,
                            Message = "Imagen encontrada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la imagen");
                return new Response<Image>
                {
                    Success = false,
                    Message = "Error al obtener la imagen",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}