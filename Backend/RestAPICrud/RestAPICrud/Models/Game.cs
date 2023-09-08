using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPICrud.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name can only be 50 characters long")]
        public string Name { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Category can only be 20 characters long")]
        public string Category { get; set; }
        [Required]
        [MaxLength(7, ErrorMessage = "Rating can only be 7 characters long")]
        public string Rating { get; set; }
        public Byte[] Picture { get; set; }
    }
}
