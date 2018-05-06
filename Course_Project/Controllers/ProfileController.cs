using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Course_Project.Data;
using Course_Project.Models;
using Course_Project.Models.ProfileViewModels;
using Course_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Course_Project.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
        private ApplicationDbContext db;
        public ProfileController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          ApplicationDbContext context,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            db = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var currentUser = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage

            };
            if (!(await _userManager.IsInRoleAsync(currentUser, "admin")) && currentUser.Id != user.Id)
            {
                ViewData["Disabled"] = "disabled";
                ViewData["Hidden"] = "display:none";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }


            if (model.Firstname != user.FirstName) { user.FirstName = model.Firstname; }
            if (model.Lastname != user.LastName) { user.LastName = model.Lastname; }
            db.Users.Update(user); await db.SaveChangesAsync();



            StatusMessage = "Your profile has been updated";
            return RedirectToAction("Index", "Manage", new { userId = user.Id });
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult SetPassword()
        {
            return View();
        }
        public IActionResult ExternalLogins()
        {
            return View();
        }
        public IActionResult MyNews()
        {
            return View();
        }
    }
}