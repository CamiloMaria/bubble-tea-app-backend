using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<Response<IEnumerable<Category>>> GetAllCategory(int page, int pageSize)
        {
            return await _categoryRepository.GetAllCategoryAsync(page, pageSize);
        }

        public async Task<Response<Category>> GetCategoryById(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task<Response<Category>> CreateCategory(Category category)
        {
            return await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task<Response<Category>> UpdateCategory(Category category)
        {
            return await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task<Response<Category>> DeleteCategory(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}