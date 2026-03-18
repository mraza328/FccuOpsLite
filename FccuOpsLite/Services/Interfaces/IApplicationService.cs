using FccuOpsLite.Models.ViewModels;

namespace FccuOpsLite.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<List<LoanApplicationListItemViewModel>> GetAllApplicationsAsync();
        Task<LoanApplicationDetailsViewModel?> GetApplicationDetailsAsync(int id);
        Task<int> CreateApplicationAsync(int memberId, decimal requestedAmount, string loanType, string notes, string actingUserId);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, string actingUserId, Models.Domain.LoanApplicationStatus newStatus, string comment);
    }
}