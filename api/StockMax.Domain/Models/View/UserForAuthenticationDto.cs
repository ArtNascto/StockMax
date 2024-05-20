using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StockMax.Domain.Models.View
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required."), JsonProperty("email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required."), JsonProperty("password")]
        public string? Password { get; set; }
    }
}