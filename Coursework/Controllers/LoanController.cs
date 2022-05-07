using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;

namespace Coursework.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // show all Loan datas
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.DvdCopy).Include(l => l.LoanType).Include(l => l.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET Loan Create view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Create()
        {
            ViewData["CopyNumber"] = new SelectList(_context.DvdCopies, "CopyNumber", "CopyNumber");
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanDuration");
            ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberFirstName");
            return View();
        }

        // POST Loan data
        // This function validate if member is less than 18 years old then he/she cannot loan dvd
        // Also if member has loan more than membership category loan the he/she cannot loan dvd 
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int LoanTypeNumber, int CopyNumber, int MemberNumber, DateTime DateOut, DateTime DateDue, DateTime DateReturned, string status, Loan loan)
        {
            loan.LoanTypeNumber = LoanTypeNumber;
            loan.CopyNumber = CopyNumber;
            loan.MemberNumber = MemberNumber;
            loan.DateOut = DateOut;
            loan.DateDue = DateDue;
            loan.DateReturned = DateReturned;
            loan.status = status;

            string loanTypeStr = HttpContext.Request.Form["LoanType.Loantype"];
            LoanType id = _context.LoanTypes.Where(l=>l.Loan_Type == loanTypeStr ).FirstOrDefault();

            Member      member = _context.Members.Where(l=>l.MemberNumber == loan.MemberNumber).Include(m=>m.MembershipCategory).FirstOrDefault();
            DVDCopy cat    = _context.DvdCopies.Where(l=>l.CopyNumber == loan.CopyNumber ).Include(c=>c.DvdTitle).ThenInclude(d=>d.DvdCategory).FirstOrDefault();

            int remainingLoanCount = _context.Loans.Where(l=> l.MemberNumber == loan.MemberNumber && l.DateReturned == null).Count();
            
            loan.DateOut = DateTime.Now;

            // loan.DateDue = DateTime.Now.AddDays(id.LoanDuration);

            if(remainingLoanCount >= member.MembershipCategory.MembershipCategoryTotalLoans)
            {
                ModelState.AddModelError(string.Empty, "Member has too many DVD unreturned!");
                return View();
            }

            if (DateTime.Today.Year  - member.MemberDateOfBirth.Year < 18 && cat.DvdTitle.DvdCategory.AgeRestricted)
            {
                ModelState.AddModelError(string.Empty, "Member is underaged for this DVD");
                return View();
            }
            try
            {
                _context.Add(loan);
                _context.SaveChanges();
                TempData["success"] = "Loan Added Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET Loan Edit view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var loan = _context.Loans.Find(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["CopyNumber"] = new SelectList(_context.DvdCopies, "CopyNumber", "CopyNumber", loan.CopyNumber);
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanDuration", loan.LoanTypeNumber);
            ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberAddress", loan.MemberNumber);
            return View(loan);
        }

        // update Loan data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int LoanTypeNumber, int CopyNumber, int MemberNumber, DateTime DateOut, DateTime DateDue, DateTime DateReturned, string status, Loan loan)
        {
            loan.LoanTypeNumber = LoanTypeNumber;
            loan.CopyNumber = CopyNumber;
            loan.MemberNumber = MemberNumber;
            loan.DateOut = DateOut;
            loan.DateDue = DateDue;
            loan.DateReturned = DateReturned;
            loan.status = status;
            try
            {
                _context.Update(loan);
                _context.SaveChanges();
                TempData["update"] = "Laon Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET Loan delete view
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.DvdCopy)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LoanNumber == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // Delete Loan data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            TempData["delete"] = "Loan Deleted Successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanNumber == id);
        }
    }
}
