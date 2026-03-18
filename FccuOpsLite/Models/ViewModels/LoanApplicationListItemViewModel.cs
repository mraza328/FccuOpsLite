using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.ViewModels
{
    public class LoanApplicationListItemViewModel
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public decimal RequestedAmount { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public LoanApplicationStatus Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}