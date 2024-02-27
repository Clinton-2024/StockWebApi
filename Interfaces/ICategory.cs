using StockWebApi.Core.Dtos.Category;

namespace StockWebApi.Interfaces
{
    public interface ICategory
    {
        Task<CategoryServiceResponse> CreateAsync(CategoryCreateDto createDto);
        Task<IEnumerable<CategoryGetAllDto>> ReadAsync();
        Task<CategoryServiceResponse> ReadByIdAsync(long id);
        Task<CategoryServiceResponse> UpdateAsync(long id, CategoryCreateDto UpdateDto);
        Task<CategoryServiceResponse> UpdateStatusAsync(long id);
        Task<CategoryServiceResponse> DeleteAsync(long id);
    }
}
