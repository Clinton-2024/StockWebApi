
namespace StockWebApi.Core.Dtos.Marchandise
{
    public class MarchandiseServiceResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public IEnumerable<MarchandiseGetAllDto>? List_Data { get; set; }
        public MarchandiseGetDto? Data { get; set; }
    }
}
