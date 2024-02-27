using System.ComponentModel.DataAnnotations;

namespace StockWebApi.Core.Entities
{
    public class Marchandise
    {
        [Key]
        public string Reference { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int Unite { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int SeuilAlerte { get; set; }
        public int StockInitial { get; set; }
        public double PrixUnitaire { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;


        // Relations

        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Operation> Operations { get; set; }







    }
}
