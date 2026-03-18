using FccuOpsLite.Models.ViewModels;
using FccuOpsLite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FccuOpsLite.Controllers
{
    [Authorize(Roles = "Admin,LoanOfficer,Processor,Viewer")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        [Authorize(Roles = "Admin,LoanOfficer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,LoanOfficer")]
        public async Task<IActionResult> Create(MemberCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _memberService.CreateMemberAsync(
                model.MemberNumber,
                model.FirstName,
                model.LastName,
                model.Email,
                model.Phone);

            TempData["SuccessMessage"] = "Member created successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}