using Newtonsoft.Json;

namespace StockMax.Domain.Models.View
{
    public class CreateUserDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uin")]
        public string Uin { get; set; }
    }
}