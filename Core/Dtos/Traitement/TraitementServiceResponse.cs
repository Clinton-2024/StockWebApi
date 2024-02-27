namespace StockWebApi.Core.Dtos.Traitement
{
    public class TraitementServiceResponse
    {
        public bool isSucceed { get; set; }
        public string? message { get; set; }
        public int total {  get; set; }
        public double valeur { get; set; }
        public string currency { get; set; }
    }
}
