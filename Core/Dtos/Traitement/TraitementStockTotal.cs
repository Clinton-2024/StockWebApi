namespace StockWebApi.Core.Dtos.Traitement
{
    public class TraitementStockTotal
    {
        public string Reference { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StockTotal { get; set; }
        public double ValeurStock { get; set; }
        public string Currency { get; set; }
    }
}
