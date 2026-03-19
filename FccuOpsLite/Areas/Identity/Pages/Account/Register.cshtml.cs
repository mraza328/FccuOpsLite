using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using FccuOpsLite.Data;
using FccuOpsLite.Models.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FccuOpsLite.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [StringLength(255)]
            [Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [Phone]
            [StringLength(25)]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 12)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var normalizedEmail = Input.Email.Trim();

            if (await _userManager.FindByEmailAsync(normalizedEmail) != null)
            {
                ModelState.AddModelError("Input.Email", "An account with this email already exists.");
                return Page();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var generatedMemberNumber = await GenerateUniqueMemberNumberAsync();

                var member = new Member
                {
                    MemberNumber = generatedMemberNumber,
                    FirstName = Input.FirstName.Trim(),
                    LastName = Input.LastName.Trim(),
                    Email = normalizedEmail,
                    Phone = Input.Phone.Trim(),
                    CreatedAtUtc = DateTime.UtcNow
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                var user = CreateUser();
                user.DisplayName = $"{member.FirstName} {member.LastName}";
                user.MemberId = member.Id;
                user.PhoneNumber = member.Phone;
                user.EmailConfirmed = true;

                await _userStore.SetUserNameAsync(user, normalizedEmail, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, normalizedEmail, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    await transaction.RollbackAsync();
                    return Page();
                }

                await _userManager.AddToRoleAsync(user, "Member");

                await transaction.CommitAsync();

                _logger.LogInformation("A new member account was created through self-registration.");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateUniqueMemberNumberAsync()
        {
            string candidate;

            do
            {
                candidate = $"M{DateTime.UtcNow:yyyyMMddHHmmss}{RandomNumberGenerator.GetInt32(100, 1000)}";
            }
            while (await _context.Members.AnyAsync(m => m.MemberNumber == candidate));

            return candidate;
        }

        private static ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException(
                    $"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}