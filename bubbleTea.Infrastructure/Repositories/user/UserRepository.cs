using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "users";
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<User>>> GetAllUserAsync(int page, int pageSize)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var totalUsers = cacheEntry?.Count ?? 0;
                var pagedUsers = cacheEntry?.Skip((page - 1) * pageSize).Take(pageSize);

                switch (pagedUsers)
                {
                    case null:
                        var message = $"No se encontraron usuarios";
                        var response = new Response<IEnumerable<User>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                }

                return new Response<IEnumerable<User>>
                {
                    Data = pagedUsers,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalUsers,
                    TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios");
                return new Response<IEnumerable<User>>
                {
                    Success = false,
                    Message = "Error al obtener los usuarios",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> GetUserByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var user = cacheEntry?.FirstOrDefault(u => u.Id == id);

                switch (user)
                {
                    case null:
                        var message = $"No se encontró el usuario con id {id}";
                        var response = new Response<User>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Id: var userId } when userId == id:
                        message = $"Se obtuvo satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                return new Response<User>
                {
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con id {id}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al obtener el usuario con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> CreateUserAsync(User user)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var existingUser = cacheEntry?.FirstOrDefault(u => u.Email == user.Email);

                switch (existingUser)
                {
                    case { Email: var email } when email == user.Email:
                        var message = $"El correo electrónico {user.Email} ya existe";
                        var response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case null:
                        message = $"Se creo satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return new Response<User>
                {
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el usuario con correo {user.Email}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al crear el usuario con correo {user.Email}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> UpdateUserAsync(User user)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var existingUser = cacheEntry?.FirstOrDefault(u => u.Id == user.Id);

                switch (existingUser)
                {
                    case null:
                        var message = $"No se encontró el usuario con id {user.Id}";
                        var response = new Response<User>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Email: var email } when email != user.Email:
                        message = $"No se puede actualizar el correo electrónico";
                        response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { Id: var id } when id == user.Id:
                        message = $"Se actualizo satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;

                await _dbContext.SaveChangesAsync();

                return new Response<User>
                {
                    Data = existingUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con id {user.Id}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al actualizar el usuario con id {user.Id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> DeleteUserAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var existingUser = cacheEntry?.FirstOrDefault(u => u.Id == id);

                switch (existingUser)
                {
                    case null:
                        var message = $"No se encontró el usuario con id {id}";
                        var response = new Response<User>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Id: var userId } when userId != id:
                        message = $"No se puede eliminar el usuario";
                        response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { Id: var userId } when userId == id:
                        message = $"Se elimino satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                _dbContext.Users.Remove(existingUser);
                await _dbContext.SaveChangesAsync();

                return new Response<User>
                {
                    Data = existingUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el usuario con id {id}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al eliminar el usuario con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> LoginUserAsync(string email, string password)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var user = cacheEntry?.FirstOrDefault(u => u.Email == email && u.Password == password);

                switch (user)
                {
                    case null:
                        var message = $"No se encontró el usuario con correo {email}";
                        var response = new Response<User>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Email: var userEmail } when userEmail != email:
                        message = $"Correo electrónico incorrecto";
                        response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { Password: var userPassword } when userPassword != password:
                        message = $"Contraseña incorrecta";
                        response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { Email: var userEmail, Password: var userPassword } when userEmail == email && userPassword == password:
                        message = $"Inicio de sesión exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                return new Response<User>
                {
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con correo {email}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al obtener el usuario con correo {email}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> RegisterUserAsync(User user)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var existingUser = cacheEntry?.FirstOrDefault(u => u.Email == user.Email);

                switch (existingUser)
                {
                    case { Email: var email } when email == user.Email:
                        var message = $"El correo electrónico {user.Email} ya existe";
                        var response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case null:
                        message = $"Se creo satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return new Response<User>
                {
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el usuario con correo {user.Email}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al crear el usuario con correo {user.Email}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }

        public async Task<Response<User>> UpdateUserPasswordAsync(int id, string oldPassword, string newPassword)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.Users.ToListAsync();
                });

                var existingUser = cacheEntry?.FirstOrDefault(u => u.Id == id);

                switch (existingUser)
                {
                    case null:
                        var message = $"No se encontró el usuario con id {id}";
                        var response = new Response<User>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Password: var password } when password != oldPassword:
                        message = $"Contraseña incorrecta";
                        response = new Response<User>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    case { Id: var userId } when userId == id:
                        message = $"Se actualizo satisfactoriamente exitoso";
                        response = new Response<User>(message, 200, "OK");
                        response.AddError(message);
                        return response;
                }

                existingUser.Password = newPassword;

                await _dbContext.SaveChangesAsync();

                return new Response<User>
                {
                    Data = existingUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con id {id}");
                return new Response<User>
                {
                    Success = false,
                    Message = $"Error al actualizar el usuario con id {id}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            };
        }
    }

}

