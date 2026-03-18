namespace FccuOpsLite.Models.Domain
{
    // Each status is assigned a specific integer value to ensure consistent storage and retrieval from the database
    public enum LoanApplicationStatus
    {
        Submitted = 1,
        UnderReview = 2,
        Approved = 3,
        Rejected = 4,
        Funded = 5
    }
}