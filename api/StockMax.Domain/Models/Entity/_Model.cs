using System.ComponentModel.DataAnnotations;

namespace StockMax.Domain.Models.Entity
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public DateTime? LastUpdate { get; set; } = DateTime.Now;
        public DateTime? DeletionTime { get; set; }
    }

    public class Model<T>
    {
        [Key]
        public T Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public DateTime? LastUpdate { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}