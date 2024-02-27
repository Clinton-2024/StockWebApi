using StockWebApi.Core.Dtos.OtherObjects;

namespace StockWebApi.Core.Dtos.Operation
{
    public class OperationEntrieCreateDto
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Type { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}
