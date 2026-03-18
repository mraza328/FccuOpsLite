using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FccuOpsLite.Models.ViewModels
{
    public class LoanApplicationCreateViewModel
    {
        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required]
        [Range(1, 1000000)]
        [Display(Name = "Requested Amount")]
        public decimal RequestedAmount { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Notes")]
        public string Notes { get; set; } = string.Empty;

        public List<SelectListItem> MemberOptions { get; set; } = new();
    }
}