using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockWebApi.Core.Entities
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long num { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public double Price {get; set;}
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string Reference { get; set; } = string.Empty;
        public Marchandise Marchandise { get; set; }
    }
}
