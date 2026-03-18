using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.Api
{
    public class LoanApplicationDetailsApiDto
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmail { get; set; } = string.Empty;
        public string MemberPhone { get; set; } = string.Empty;
        public decimal RequestedAmount { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public LoanApplicationStatus Status { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public List<LoanApplicationHistoryApiDto> StatusHistory { get; set; } = new();
    }
}