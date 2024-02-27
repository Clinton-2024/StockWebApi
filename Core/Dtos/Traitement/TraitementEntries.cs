namespace StockWebApi.Core.Dtos.Traitement
{
    public class TraitementEntries
    {
        public string Reference { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalEntries { get; set; }
        public double ValeurEntries { get; set; }
        public string Currency { get; set; }
    }
}
