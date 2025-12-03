using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using greengrocer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace greengrocer.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly LoginOptions _loginOptions;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IOptions<LoginOptions> loginOptions, ILogger<LoginModel> logger)
        {
            _loginOptions = loginOptions.Value;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var validUser = _loginOptions.ValidUsername;
            var validPass = _loginOptions.ValidPassword;

            if (Input.Username == validUser && Input.Password == validPass)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.Username),
                    new Claim(ClaimTypes.NameIdentifier, Input.Username)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = true,                      
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) 
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProps);

                _logger.LogInformation("User {Username} successfully logged in.", Input.Username);

                return RedirectToPage("/Order");
            }

            _logger.LogWarning("Failed login attempt for username: {Username}", Input.Username);
            
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return Page();
        }
    }
}
