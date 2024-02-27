
namespace StockWebApi.Core.Dtos.Operation
{
    public class OperationServiceResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public IEnumerable<OperationGetAllDto>? List_Data { get; set; }
        public OperationGetAllDto? Data { get; set; }
    }
}
