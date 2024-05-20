using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StockMax.Domain.Models.View
{
    public class UserDto
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [MaxLength(255), Required, JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [Required, JsonProperty("informations")]
        public string Informations { get; set; } = string.Empty;

        [Required, JsonProperty("gender")]
        public string Gender { get; set; } = string.Empty;

        [Required, JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [Required, JsonProperty("uin")]
        public string Uin { get; set; } = string.Empty;

        [Required, JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [Required, JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [Required, JsonProperty("zipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [Required, JsonProperty("address")]
        public string Address { get; set; } = string.Empty;

        [Required, JsonProperty("addressNumber")]
        public string AddressNumber { get; set; } = string.Empty;

        [Required, JsonProperty("neighborhood")]
        public string Neighborhood { get; set; } = string.Empty;

        [Required, JsonProperty("complement")]
        public string Complement { get; set; } = string.Empty;

        [Required, JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [Required, JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [JsonProperty("maritalStatus")]
        public string MaritalStatus { get; set; } = string.Empty;

        [JsonProperty("job")]
        public string Job { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("imagePath")]
        public string ImagePath { get; set; } = string.Empty;

        [JsonProperty("lastAccess")]
        public virtual DateTime? LastAccess { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("deletionTime")]
        public DateTime? DeletionTime { get; set; }

        [JsonProperty("emailConfirmed")]
        public virtual bool? EmailConfirmed { get; set; }
    }
}