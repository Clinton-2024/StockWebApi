using System.ComponentModel.DataAnnotations;

namespace StockWebApi.Core.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required, MinLength(8, ErrorMessage = "Please enter at least 8 characteres")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "FirstName is required")]
        public string SousDomaine { get; set; } = string.Empty;

    }
}
