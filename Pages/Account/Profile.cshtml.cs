using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassiveMoneyTracker.Data;
using PassiveMoneyTracker.Models;

namespace PassiveMoneyTracker.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public User User { get; set; }
        public List<PassiveIncome> PassiveIncomeRecords { get; set; }

        [BindProperty]
        public PassiveIncome NewIncome { get; set; }

        public void OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                User = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

                if (User != null)
                {
                    PassiveIncomeRecords = _context.PassiveIncomes
                        .Where(pi => pi.UserId == User.Id)
                        .OrderByDescending(pi => pi.Date)
                        .ToList();
                }
            }
            //else
            //{
            //    RedirectToPage("/Account/Login");
            //}
        }


        public IActionResult OnPost()
        {
            if (NewIncome != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    NewIncome.UserId = userId.Value;
                    _context.PassiveIncomes.Add(NewIncome);
                    _context.SaveChanges();
                }
            }

            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("UserId");

            return RedirectToPage("/Index");
        }
        public IActionResult OnPostCalculateAverage()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var averageIncome = _context.PassiveIncomes
                    .Where(pi => pi.UserId == userId.Value)
                    .Average(pi => pi.Amount);
            // #BUG Raise message if no passive income records are present

                TempData["AverageIncome"] = averageIncome.ToString("F2");
            }

            return RedirectToPage();
        }


    }
}
