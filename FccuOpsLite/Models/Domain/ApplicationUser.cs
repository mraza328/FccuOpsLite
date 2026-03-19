using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FccuOpsLite.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public int? MemberId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }
    }
}