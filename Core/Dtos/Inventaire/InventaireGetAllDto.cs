namespace StockWebApi.Core.Dtos.Inventaire
{
    public class InventaireGetAllDto 
    {
        public string Reference { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Unite { get; set; } = string.Empty;
        public int SeuilAlerte { get; set; }
        public int StockInitial { get; set; }
        public double Entries { get; set; }
        public double Output { get; set; }
        public int StockFinal { get; set; }
        public double CUMP { get; set; }
        public double Valeur { get; set; }
        public string Statut { get; set; }
        
        
    }
}
