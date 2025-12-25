using System.ComponentModel.DataAnnotations;

namespace asp.net_jwt.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public DateTime RefreshTokenExpires { get; set; } = DateTime.Now;

        [Required]
        public bool IsEnable { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
