using System.ComponentModel.DataAnnotations;

namespace StockWebApi.Core.Dtos.Auth
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
