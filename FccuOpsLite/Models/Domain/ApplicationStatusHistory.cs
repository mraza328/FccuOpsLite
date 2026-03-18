using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FccuOpsLite.Models.Domain
{
    public class ApplicationStatusHistory
    {
        public int Id { get; set; }

        [Required]
        public int LoanApplicationId { get; set; }

        [ForeignKey(nameof(LoanApplicationId))]
        public LoanApplication? LoanApplication { get; set; }

        [Required]
        public LoanApplicationStatus OldStatus { get; set; }

        [Required]
        public LoanApplicationStatus NewStatus { get; set; }

        [Required]
        [StringLength(450)]
        public string ChangedByUserId { get; set; } = string.Empty;

        [StringLength(500)]
        public string Comment { get; set; } = string.Empty;

        public DateTime ChangedAtUtc { get; set; } = DateTime.UtcNow;
    }
}