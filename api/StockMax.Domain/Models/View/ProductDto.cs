using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockMax.Domain.Models.View
{
    public class ProductDto
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, JsonProperty("creationTime"), JsonPropertyName("creationTime")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [JsonProperty("lastUpdate"), JsonPropertyName("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("deletionTime"), JsonPropertyName("deletionTime")]
        public DateTime? DeletionTime { get; set; }

        [MaxLength(255), Required, JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [Required, JsonProperty("quantity"), JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;

        [JsonProperty("imagePath"), JsonPropertyName("imagePath")]
        public string? ImagePath { get; set; } = "";

        [JsonProperty("description"), JsonPropertyName("description")]
        public string? Description { get; set; } = "";

        [Required, JsonProperty("colors"), JsonPropertyName("colors")]
        public string Colors { get; set; }

        [Required, JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; }

        [Required, JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("vendor"), JsonPropertyName("vendor")]
        public string? Vendor { get; set; }

        [JsonProperty("label"), JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonProperty("category"), JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonProperty("value"), JsonPropertyName("value")]
        public decimal Value { get; set; } = 0;
    }
}