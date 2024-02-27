namespace StockWebApi.Core.Dtos.Traitement
{
    public class TraitementOutput
    {

        public string Reference { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalOutput { get; set; }
        public double ValeurOutput { get; set; }
        public string Currency { get; set; }
    }
}
