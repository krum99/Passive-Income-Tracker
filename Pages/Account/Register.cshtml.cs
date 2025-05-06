using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassiveMoneyTracker.Data;
using PassiveMoneyTracker.Models;

namespace PassiveMoneyTracker.Pages.Account
{
    public class RegisterModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public string Message { get; set; }

        public IActionResult OnPost()
        {
            if (_context.Users.Any(u => u.Username == User.Username))
            {
                Message = "Username already exists!";
                return Page();
            }

            _context.Users.Add(User);
            _context.SaveChanges();

            return RedirectToPage("/Account/Login");
        }
        public void OnGet()
        {

        }
    }
}

//public class RegisterModel : PageModel
//{
    
//}

