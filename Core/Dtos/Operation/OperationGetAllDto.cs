namespace StockWebApi.Core.Dtos.Operation
{
    public class OperationGetAllDto
    {
        public long num { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int Unite { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
