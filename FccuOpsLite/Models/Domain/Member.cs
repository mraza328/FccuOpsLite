using System.ComponentModel.DataAnnotations;

namespace FccuOpsLite.Models.Domain
{
    public class Member
    {
        // Primary key for the Member entity, auto-incremented by the database
        public int Id { get; set; }

        // Property is required and has a maximum length of 20 characters to ensure data integrity and prevent excessively long member numbers
        [Required]
        [StringLength(20)]
        public string MemberNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(25)]
        public string Phone { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation property to represent the one-to-many relationship between Member and LoanApplication
        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
    }
}