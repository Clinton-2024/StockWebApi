namespace StockWebApi.Core.Dtos.Category
{
    public class CategoryGetAllDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int NombreArticles { get; set; }
        public bool IsActive { get; set; } 
    }
}
