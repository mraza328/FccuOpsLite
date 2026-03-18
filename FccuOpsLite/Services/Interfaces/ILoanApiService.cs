using FccuOpsLite.Models.Api;

namespace FccuOpsLite.Services.Interfaces
{
    public interface ILoanApiService
    {
        Task<List<LoanApplicationSummaryApiDto>> GetLoanApplicationsAsync(LoanApplicationQueryApiDto query);
        Task<LoanApplicationDetailsApiDto?> GetLoanApplicationByIdAsync(int id);
    }
}