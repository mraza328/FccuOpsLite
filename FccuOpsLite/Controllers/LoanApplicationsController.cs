using FccuOpsLite.Models.Domain;
using FccuOpsLite.Models.ViewModels;
using FccuOpsLite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FccuOpsLite.Controllers
{
    [Authorize(Roles = "Admin,LoanOfficer,Processor,Viewer")]
    public class LoanApplicationsController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IMemberService _memberService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanApplicationsController(
            IApplicationService applicationService,
            IMemberService memberService,
            UserManager<ApplicationUser> userManager)
        {
            _applicationService = applicationService;
            _memberService = memberService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return View(applications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicationService.GetApplicationDetailsAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [Authorize(Roles = "Admin,LoanOfficer")]
        public async Task<IActionResult> Create()
        {
            var model = new LoanApplicationCreateViewModel();
            await PopulateMembersAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,LoanOfficer")]
        public async Task<IActionResult> Create(LoanApplicationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateMembersAsync(model);
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Forbid();
            }

            var applicationId = await _applicationService.CreateApplicationAsync(
                model.MemberId,
                model.RequestedAmount,
                model.LoanType,
                model.Notes,
                userId);

            TempData["SuccessMessage"] = "Loan application created successfully.";
            return RedirectToAction(nameof(Details), new { id = applicationId });
        }

        [Authorize(Roles = "Admin,Processor")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var application = await _applicationService.GetApplicationDetailsAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            var model = new UpdateLoanApplicationStatusViewModel
            {
                Id = application.Id,
                CurrentStatus = application.Status,
                NewStatus = application.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Processor")]
        public async Task<IActionResult> UpdateStatus(UpdateLoanApplicationStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Forbid();
            }

            try
            {
                var updated = await _applicationService.UpdateApplicationStatusAsync(
                    model.Id,
                    userId,
                    model.NewStatus,
                    model.Comment);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Application status updated successfully.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        private async Task PopulateMembersAsync(LoanApplicationCreateViewModel model)
        {
            var members = await _memberService.GetAllMembersAsync();

            model.MemberOptions = members
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.MemberNumber} - {m.FirstName} {m.LastName}"
                })
                .ToList();
        }
    }
}