using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<IEnumerable<Category>>> GetAllCategory();
        Task<Response<Category>> GetCategoryById(int id);
        Task<Response<Category>> CreateCategory(Category category);
        Task<Response<Category>> UpdateCategory(Category category);
        Task<Response<Category>> DeleteCategory(int id);
    }
}