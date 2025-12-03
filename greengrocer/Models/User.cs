using System;
using System.ComponentModel.DataAnnotations;

namespace greengrocer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }   

        [Required]
        public string Password { get; set; }   

        [MaxLength(150)]
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
