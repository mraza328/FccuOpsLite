using FccuOpsLite.Models.Domain;

namespace FccuOpsLite.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task<int> CreateMemberAsync(string memberNumber, string firstName, string lastName, string email, string phone);
    }
}