using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Response<IEnumerable<Category>>> GetAllCategoryAsync();
        Task<Response<Category>> GetCategoryByIdAsync(int id);
        Task<Response<Category>> CreateCategoryAsync(Category category);
        Task<Response<Category>> UpdateCategoryAsync(Category category);
        Task<Response<Category>> DeleteCategoryAsync(int id);
    }
}