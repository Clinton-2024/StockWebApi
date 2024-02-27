
namespace StockWebApi.Core.Dtos.Inventaire
{
    public class InventaireServiceResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public IEnumerable<InventaireGetAllDto>? List_Data { get; set; }
        public InventaireGetAllDto? Data { get; set; }
    }
}
