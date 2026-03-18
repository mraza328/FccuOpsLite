using FccuOpsLite.Data;
using FccuOpsLite.Models.Domain;
using FccuOpsLite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FccuOpsLite.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> CreateMemberAsync(string memberNumber, string firstName, string lastName, string email, string phone)
        {
            var member = new Member
            {
                MemberNumber = memberNumber.Trim(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim(),
                Phone = phone.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return member.Id;
        }
    }
}