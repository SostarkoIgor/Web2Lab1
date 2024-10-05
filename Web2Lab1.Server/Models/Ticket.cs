using System.ComponentModel.DataAnnotations;

namespace Web2Lab1.Server.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        public string? Vatin { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
