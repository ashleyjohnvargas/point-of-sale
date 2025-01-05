using System.ComponentModel.DataAnnotations;

namespace POS1.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }
}