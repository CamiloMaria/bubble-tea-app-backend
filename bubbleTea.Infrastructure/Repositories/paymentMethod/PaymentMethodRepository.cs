using Microsoft.EntityFrameworkCore;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BubbleTea.Infrastructure.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "paymentMethods";
        private readonly ILogger<PaymentMethodRepository> _logger;

        public PaymentMethodRepository(ApplicationDbContext dbContext, IMemoryCache cache, ILogger<PaymentMethodRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<PaymentMethod>>> GetAllPaymentMethodAsync()
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.ToListAsync();
                });

                switch (cacheEntry)
                {
                    case null:
                        var message = "No se encontraron métodos de pago";
                        var response = new Response<IEnumerable<PaymentMethod>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<PaymentMethod>>
                        {
                            Data = cacheEntry,
                            Success = true,
                            Message = "Metodos de pago encontrados",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los métodos de pago");
                return new Response<IEnumerable<PaymentMethod>>
                {
                    Success = false,
                    Message = "Error al obtener los métodos de pago",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<PaymentMethod>> GetPaymentMethodByIdAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.FirstOrDefaultAsync(x => x.Id == id);
                });

                switch (cacheEntry)
                {
                    case null:
                        var message = "No se encontró el método de pago";
                        var response = new Response<PaymentMethod>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<PaymentMethod>
                        {
                            Data = cacheEntry,
                            Success = true,
                            Message = "Método de pago encontrado",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el método de pago");
                return new Response<PaymentMethod>
                {
                    Success = false,
                    Message = "Error al obtener el método de pago",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.ToListAsync();
                });

                var existingPaymentMethod = cacheEntry?.FirstOrDefault(x => x.Name == paymentMethod.Name);

                switch (existingPaymentMethod)
                {
                    case { Name: var name } when name == paymentMethod.Name:
                        var message = "Ya existe un método de pago con ese nombre";
                        var response = new Response<PaymentMethod>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        await _dbContext.PaymentMethods.AddAsync(paymentMethod);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<PaymentMethod>
                        {
                            Data = paymentMethod,
                            Success = true,
                            Message = "Método de pago creado",
                            StatusCode = 201,
                            ReasonPhrase = "Created"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el método de pago");
                return new Response<PaymentMethod>
                {
                    Success = false,
                    Message = "Error al crear el método de pago",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<PaymentMethod>> UpdatePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.ToListAsync();
                });

                var existingPaymentMethod = cacheEntry?.FirstOrDefault(x => x.Id == paymentMethod.Id);

                switch (existingPaymentMethod)
                {
                    case null:
                        var message = "No se encontró el método de pago";
                        var response = new Response<PaymentMethod>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { Name: var name } when name == paymentMethod.Name:
                        message = "Ya existe un método de pago con ese nombre";
                        response = new Response<PaymentMethod>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.Entry(existingPaymentMethod).State = EntityState.Detached;
                        _dbContext.Entry(paymentMethod).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<PaymentMethod>
                        {
                            Data = existingPaymentMethod,
                            Success = true,
                            Message = "Método de pago actualizado",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el método de pago");
                return new Response<PaymentMethod>
                {
                    Success = false,
                    Message = "Error al actualizar el método de pago",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<PaymentMethod>> DeletePaymentMethodAsync(int id)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.ToListAsync();
                });

                var existingPaymentMethod = cacheEntry?.FirstOrDefault(x => x.Id == id);

                switch (existingPaymentMethod)
                {
                    case null:
                        var message = "No se encontró el método de pago";
                        var response = new Response<PaymentMethod>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    case { } when existingPaymentMethod.Orders.Any():
                        message = "No se puede eliminar el método de pago porque tiene órdenes asociadas";
                        response = new Response<PaymentMethod>(message, 400, "Bad Request");
                        response.AddError(message);
                        return response;
                    default:
                        _dbContext.PaymentMethods.Remove(existingPaymentMethod);
                        await _dbContext.SaveChangesAsync();

                        _cache.Remove(_cacheKey);

                        return new Response<PaymentMethod>
                        {
                            Data = existingPaymentMethod,
                            Success = true,
                            Message = "Método de pago eliminado",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el método de pago");
                return new Response<PaymentMethod>
                {
                    Success = false,
                    Message = "Error al eliminar el método de pago",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }

        public async Task<Response<IEnumerable<PaymentMethod>>> GetPaymentMethodByOrderIdAsync(int orderId)
        {
            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(_cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    return await _dbContext.PaymentMethods.ToListAsync();
                });

                var paymentMethods = cacheEntry?.Where(x => x.OrderId == orderId);

                switch (paymentMethods)
                {
                    case null:
                        var message = $"No se encontraron métodos de pago con ese id de orden {orderId}";
                        var response = new Response<IEnumerable<PaymentMethod>>(message, 404, "Not Found");
                        response.AddError(message);
                        return response;
                    default:
                        return new Response<IEnumerable<PaymentMethod>>
                        {
                            Data = paymentMethods,
                            Success = true,
                            Message = "Métodos de pago encontrados",
                            StatusCode = 200,
                            ReasonPhrase = "OK"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los métodos de pago con ese id de orden {orderId}", orderId);
                return new Response<IEnumerable<PaymentMethod>>
                {
                    Success = false,
                    Message = $"Error al obtener los métodos de pago con ese id de orden {orderId}",
                    StatusCode = 500,
                    ReasonPhrase = "Internal Server Error"
                };
            }
        }
    }
}