using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StockMax.Domain.Models.Entity
{
    public class User : IdentityUser
    {
        [MaxLength(255), Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Informations { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Uin { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string AddressNumber { get; set; } = string.Empty;

        [Required]
        public string Neighborhood { get; set; } = string.Empty;

        [Required]
        public string Complement { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        public string MaritalStatus { get; set; } = string.Empty;

        public string Job { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public virtual DateTime? LastAccess { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public DateTime? LastUpdate { get; set; }
        public DateTime? DeletionTime { get; set; }
    }

    public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();

            // Sua lógica de validação de e-mail aqui (pode ser mais flexível)
            var email = await manager.GetEmailAsync(user);
            if (!string.IsNullOrEmpty(email))
                if (!IsValidEmail(email))
                {
                    errors.Add(new IdentityError { Description = "Email inválido." });
                }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        private bool IsValidEmail(string email)
        {
            return email != null && email.Contains("@");
        }
    }
}