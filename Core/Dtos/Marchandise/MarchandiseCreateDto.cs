
namespace StockWebApi.Core.Dtos.Marchandise
{
    public class MarchandiseCreateDto
    {
        public string Reference { get; set; } 
        public string Photo { get; set; }
        public string Designation { get; set; } 
        public int Unite { get; set; }
        public string Currency { get; set; }
        public int SeuilAlerte { get; set; }
        public int StockInitial { get; set; }
        public double PrixUnitaire { get; set; }
        public long CategoryId { get; set; }
    }
}
