using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FccuOpsLite.Models.Domain
{
    public class LoanApplication
    {
        public int Id { get; set; }

        // Foreign key to the Member entity, ensuring that each loan application is associated with a valid member
        [Required]
        public int MemberId { get; set; }

        // Related object to represent the relationship between LoanApplication and Member, allowing for easy access to member details from a loan application
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }

        [Required]
        [Range(1, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RequestedAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanType { get; set; } = string.Empty;

        [Required]
        public LoanApplicationStatus Status { get; set; } = LoanApplicationStatus.Submitted;

        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        public ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();
    }
}