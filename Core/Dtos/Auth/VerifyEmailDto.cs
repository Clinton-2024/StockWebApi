using System.ComponentModel.DataAnnotations;

namespace StockWebApi.Core.Dtos.Auth
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
