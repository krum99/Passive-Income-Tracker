using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassiveMoneyTracker.Data;

namespace PassiveMoneyTracker.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public IActionResult OnPost()
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user == null)
            {
                Message = "Invalid username or password";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage("/Account/Profile");
        }
        public void OnGet()
        {
        }
    }
}
