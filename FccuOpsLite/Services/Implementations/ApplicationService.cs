using FccuOpsLite.Data;
using FccuOpsLite.Models.Domain;
using FccuOpsLite.Models.ViewModels;
using FccuOpsLite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FccuOpsLite.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LoanApplicationListItemViewModel>> GetAllApplicationsAsync()
        {
            return await _context.LoanApplications
                .Include(a => a.Member)
                .OrderByDescending(a => a.CreatedAtUtc)
                .Select(a => new LoanApplicationListItemViewModel
                {
                    Id = a.Id,
                    MemberNumber = a.Member != null ? a.Member.MemberNumber : string.Empty,
                    MemberName = a.Member != null ? $"{a.Member.FirstName} {a.Member.LastName}" : string.Empty,
                    RequestedAmount = a.RequestedAmount,
                    LoanType = a.LoanType,
                    Status = a.Status,
                    CreatedAtUtc = a.CreatedAtUtc
                })
                .ToListAsync();
        }

        public async Task<LoanApplicationDetailsViewModel?> GetApplicationDetailsAsync(int id)
        {
            var application = await _context.LoanApplications
                .Include(a => a.Member)
                .Include(a => a.StatusHistory)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null || application.Member == null)
            {
                return null;
            }

            return new LoanApplicationDetailsViewModel
            {
                Id = application.Id,
                MemberNumber = application.Member.MemberNumber,
                MemberName = $"{application.Member.FirstName} {application.Member.LastName}",
                MemberEmail = application.Member.Email,
                MemberPhone = application.Member.Phone,
                RequestedAmount = application.RequestedAmount,
                LoanType = application.LoanType,
                Status = application.Status,
                Notes = application.Notes,
                CreatedAtUtc = application.CreatedAtUtc,
                UpdatedAtUtc = application.UpdatedAtUtc,
                StatusHistory = application.StatusHistory
                    .OrderByDescending(h => h.ChangedAtUtc)
                    .Select(h => new ApplicationStatusHistoryItemViewModel
                    {
                        ChangedByUserId = h.ChangedByUserId,
                        OldStatus = h.OldStatus,
                        NewStatus = h.NewStatus,
                        Comment = h.Comment,
                        ChangedAtUtc = h.ChangedAtUtc
                    })
                    .ToList()
            };
        }

        public async Task<int> CreateApplicationAsync(int memberId, decimal requestedAmount, string loanType, string notes, string actingUserId)
        {
            var memberExists = await _context.Members.AnyAsync(m => m.Id == memberId);

            if (!memberExists)
            {
                throw new InvalidOperationException("Selected member does not exist.");
            }

            var application = new LoanApplication
            {
                MemberId = memberId,
                RequestedAmount = requestedAmount,
                LoanType = loanType.Trim(),
                Notes = notes.Trim(),
                Status = LoanApplicationStatus.Submitted,
                CreatedAtUtc = DateTime.UtcNow
            };

            _context.LoanApplications.Add(application);
            await _context.SaveChangesAsync();

            var history = new ApplicationStatusHistory
            {
                LoanApplicationId = application.Id,
                OldStatus = LoanApplicationStatus.Submitted,
                NewStatus = LoanApplicationStatus.Submitted,
                ChangedByUserId = actingUserId,
                Comment = "Application created.",
                ChangedAtUtc = DateTime.UtcNow
            };

            _context.ApplicationStatusHistories.Add(history);
            await _context.SaveChangesAsync();

            return application.Id;
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, string actingUserId, LoanApplicationStatus newStatus, string comment)
        {
            var application = await _context.LoanApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
            {
                return false;
            }

            var currentStatus = application.Status;

            if (!IsAllowedTransition(currentStatus, newStatus))
            {
                throw new InvalidOperationException($"Transition from {currentStatus} to {newStatus} is not allowed.");
            }

            application.Status = newStatus;
            application.UpdatedAtUtc = DateTime.UtcNow;

            var history = new ApplicationStatusHistory
            {
                LoanApplicationId = application.Id,
                OldStatus = currentStatus,
                NewStatus = newStatus,
                ChangedByUserId = actingUserId,
                Comment = comment.Trim(),
                ChangedAtUtc = DateTime.UtcNow
            };

            _context.ApplicationStatusHistories.Add(history);
            await _context.SaveChangesAsync();

            return true;
        }

        private static bool IsAllowedTransition(LoanApplicationStatus currentStatus, LoanApplicationStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                return false;
            }

            return currentStatus switch
            {
                LoanApplicationStatus.Submitted => newStatus is LoanApplicationStatus.UnderReview or LoanApplicationStatus.Rejected,
                LoanApplicationStatus.UnderReview => newStatus is LoanApplicationStatus.Approved or LoanApplicationStatus.Rejected,
                LoanApplicationStatus.Approved => newStatus is LoanApplicationStatus.Funded,
                LoanApplicationStatus.Rejected => false,
                LoanApplicationStatus.Funded => false,
                _ => false
            };
        }
    }
}