using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.ViewModels
{
    public class LoanApplicationDetailsViewModel
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

        public List<ApplicationStatusHistoryItemViewModel> StatusHistory { get; set; } = new();
    }

    public class ApplicationStatusHistoryItemViewModel
    {
        public string ChangedByUserId { get; set; } = string.Empty;
        public LoanApplicationStatus OldStatus { get; set; }
        public LoanApplicationStatus NewStatus { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ChangedAtUtc { get; set; }
    }
}