using FccuOpsLite.Data;
using FccuOpsLite.Models.Api;
using FccuOpsLite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FccuOpsLite.Services.Implementations
{
    public class LoanApiService : ILoanApiService
    {
        private readonly ApplicationDbContext _context;

        public LoanApiService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LoanApplicationSummaryApiDto>> GetLoanApplicationsAsync(LoanApplicationQueryApiDto query)
        {
            var applicationsQuery = _context.LoanApplications
                .AsNoTracking()
                .Include(a => a.Member)
                .AsQueryable();

            if (query.Status.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(a => a.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.MemberNumber))
            {
                var memberNumber = query.MemberNumber.Trim();
                applicationsQuery = applicationsQuery.Where(a =>
                    a.Member != null && a.Member.MemberNumber == memberNumber);
            }

            if (query.MinAmount.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(a => a.RequestedAmount >= query.MinAmount.Value);
            }

            if (query.MaxAmount.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(a => a.RequestedAmount <= query.MaxAmount.Value);
            }

            return await applicationsQuery
                .OrderByDescending(a => a.CreatedAtUtc)
                .Select(a => new LoanApplicationSummaryApiDto
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

        public async Task<LoanApplicationDetailsApiDto?> GetLoanApplicationByIdAsync(int id)
        {
            var application = await _context.LoanApplications
                .AsNoTracking()
                .Include(a => a.Member)
                .Include(a => a.StatusHistory)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null || application.Member == null)
            {
                return null;
            }

            return new LoanApplicationDetailsApiDto
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
                    .Select(h => new LoanApplicationHistoryApiDto
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
    }
}