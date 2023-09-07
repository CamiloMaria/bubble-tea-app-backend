using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "categories";
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<CategoryRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Category>>> GetAllCategoryAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Categories.ToListAsync();
                });

                var totalCategories = cacheEntry?.Count ?? 0;
                var pagedCategories = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                switch (pagedCategories)
                {
                    case null:
                        var message = $"No se encontraron categorias con {page} y {pageSize}";
                        var response = new Response<IEnumerable<Category>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<Category>>
                        {
                            Data = pagedCategories,
                            Page = page,
                            PageSize = pageSize,
                            TotalCount = totalCategories,
                            TotalPages = (int)Math.Ceiling((decimal)totalCategories / pageSize),
                            Success = true,
                            Message = "Categorias encontradas",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorias");
                return new Response<IEnumerable<Category>>
                {
                    Success = false,
                    Message = "Error al obtener las categorias",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Category>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Categories.ToListAsync();
                });

                var category = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (category)
                {
                    case null:
                        var message = $"No se encontro la categoria con el id {id}";
                        var response = new Response<Category>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<Category>
                        {
                            Data = category,
                            Success = true,
                            Message = "Categoria encontrada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la categoria");
                return new Response<Category>
                {
                    Success = false,
                    Message = "Error al obtener la categoria",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Category>> CreateCategoryAsync(Category category)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Categories.ToListAsync();
                });

                var existingCategory = cacheEntry?.FirstOrDefault(x => x.Name == category.Name);

                switch (existingCategory)
                {
                    case { Name: var name } when name == category.Name:
                        var message = $"Ya existe una categoria con el nombre {category.Name}";
                        var response = new Response<Category>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.Categories.AddAsync(category);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Category>
                        {
                            Data = category,
                            Success = true,
                            Message = "Categoria creada",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoria");
                return new Response<Category>
                {
                    Success = false,
                    Message = "Error al crear la categoria",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Category>> UpdateCategoryAsync(Category category)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Categories.ToListAsync();
                });

                var existingCategory = cacheEntry?.FirstOrDefault(x => x.Id == category.Id);

                switch (existingCategory)
                {
                    case null:
                        var message = $"No se encontro la categoria con el id {category.Id}";
                        var response = new Response<Category>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Categories.Entry(existingCategory).State = EntityState.Detached;
                        _dbContext.Categories.Entry(category).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Category>
                        {
                            Data = existingCategory,
                            Success = true,
                            Message = "Categoria actualizada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoria");
                return new Response<Category>
                {
                    Success = false,
                    Message = "Error al actualizar la categoria",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<Category>> DeleteCategoryAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Categories.ToListAsync();
                });

                var existingCategory = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingCategory)
                {
                    case null:
                        var message = $"No se encontro la categoria con el id {id}";
                        var response = new Response<Category>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when existingCategory.Products.Any():
                        message = $"No se puede eliminar la categoria con el id {id} porque tiene productos asociados";
                        response = new Response<Category>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Categories.Remove(existingCategory);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<Category>
                        {
                            Data = existingCategory,
                            Success = true,
                            Message = "Categoria eliminada",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoria");
                return new Response<Category>
                {
                    Success = false,
                    Message = "Error al eliminar la categoria",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}
