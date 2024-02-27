
namespace StockWebApi.Core.Dtos.Category
{
    public class CategoryServiceResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public IEnumerable<CategoryGetAllDto>? List_Data { get; set; }
        public CategoryGetAllDto? Data { get; set; }
    }
}
