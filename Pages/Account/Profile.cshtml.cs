﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public List<PassiveIncome> PassiveIncomeRecords { get; set; }

        [BindProperty]
        public PassiveIncome NewIncome { get; set; }

        [BindProperty]
        public PassiveIncome EditIncome { get; set; }

        public async Task<IActionResult> OnPostSaveRecordAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            User = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            // BUG: Something is causing the userId always to be '0',
            // nonetheless its being set correctly in 'OnPostEditAsync' method.
            EditIncome.UserId = userId ?? 0;

            _context.PassiveIncomes.Update(EditIncome);
            await _context.SaveChangesAsync();
            //EditIncome = null;
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            User = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            EditIncome = await _context.PassiveIncomes.FindAsync(id);
            if (EditIncome == null)
                return RedirectToPage();
            return Page();
        }


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

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var income = await _context.PassiveIncomes.FindAsync(id);
            if (income != null)
            {
                _context.PassiveIncomes.Remove(income);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToPage("/Account/Login");

            User = await _context.Users.FindAsync(userId.Value);

            var query = _context.PassiveIncomes.Where(pi => pi.UserId == User.Id);
            
            if (!string.IsNullOrWhiteSpace(SearchTerm))
                query = query.Where(pi => pi.Source.Contains(SearchTerm));

            PassiveIncomeRecords = await query
                .OrderByDescending(pi => pi.Date)
                .ToListAsync();

            return Page();
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
