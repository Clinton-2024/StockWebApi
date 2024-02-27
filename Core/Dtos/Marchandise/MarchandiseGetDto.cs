namespace StockWebApi.Core.Dtos.Marchandise
{
    public class MarchandiseGetDto
    {
        public string Reference { get; set; }
        public string Photo { get; set; }
        public string Designation { get; set; }
        public int Unite { get; set; }
        public string Currency { get; set; }
        public int SeuilAlerte { get; set; }
        public int StockInitial { get; set; }
        public double PrixUnitaire { get; set; }
        public double Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
