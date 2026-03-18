using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Models.Api
{
    public class LoanApplicationHistoryApiDto
    {
        public string ChangedByUserId { get; set; } = string.Empty;
        public LoanApplicationStatus OldStatus { get; set; }
        public LoanApplicationStatus NewStatus { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ChangedAtUtc { get; set; }
    }
}