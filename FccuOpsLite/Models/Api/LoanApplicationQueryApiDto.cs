using System.ComponentModel.DataAnnotations;
using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.Api
{
    public class LoanApplicationQueryApiDto
    {
        public LoanApplicationStatus? Status { get; set; }

        [StringLength(20)]
        public string MemberNumber { get; set; } = string.Empty;

        [Range(0, 1000000)]
        public decimal? MinAmount { get; set; }

        [Range(0, 1000000)]
        public decimal? MaxAmount { get; set; }
    }
}