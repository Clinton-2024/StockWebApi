namespace StockWebApi.Core.Dtos.Traitement
{
    public class RepartitionStock
    {
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }
        public int Quantity { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
    }
}
