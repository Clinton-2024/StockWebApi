namespace StockWebApi.Core.Dtos.Traitement
{
    public class RepartitionServiceResponse
    {
        public bool isSucceed { get; set; }
        public string? message { get; set; }
        public string Category { get; set; }
        public int? Quantity { get; set; }
        public int? Total { get; set; }
        public string currency { get; set; }
    }
}
