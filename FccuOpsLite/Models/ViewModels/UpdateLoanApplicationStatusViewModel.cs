using System.ComponentModel.DataAnnotations;
using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.ViewModels
{
    public class UpdateLoanApplicationStatusViewModel
    {
        [Required]
        public int Id { get; set; }

        public LoanApplicationStatus CurrentStatus { get; set; }

        [Required]
        [Display(Name = "New Status")]
        public LoanApplicationStatus NewStatus { get; set; }

        [StringLength(500)]
        [Display(Name = "Comment")]
        public string Comment { get; set; } = string.Empty;
    }
}