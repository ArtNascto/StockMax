using Newtonsoft.Json;

namespace StockMax.Domain.Models.View
{
    public class AuthResponseDto
    {
        [JsonProperty("isAuthSuccessful")]
        public bool IsAuthSuccessful { get; set; }

        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; } = "admin";

        [JsonProperty("lastAccess")]
        public DateTime? LastAccess { get; set; }
    }
}