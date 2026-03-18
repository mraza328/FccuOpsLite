using System.ComponentModel.DataAnnotations;

namespace FccuOpsLite.Models.ViewModels
{
    public class MemberCreateViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Member Number")]
        public string MemberNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(25)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;
    }
}