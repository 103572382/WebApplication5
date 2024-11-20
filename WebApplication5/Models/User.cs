using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public string Address { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Ethnicity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
