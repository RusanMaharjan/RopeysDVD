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
    public class MembershipCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET MembershipCategory details
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.MembershipCategories.ToListAsync());
        }

        // show MembershipCategory Create view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Create()
        {
            return View();
        }

        // store MembershipCategory data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipCategoryNumber,MembershipCategoryDescription,MembershipCategoryTotalLoans")] MembershipCategory membershipCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipCategory);
        }

        // show MembershipCategory Edit data
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipCategory = await _context.MembershipCategories.FindAsync(id);
            if (membershipCategory == null)
            {
                return NotFound();
            }
            return View(membershipCategory);
        }

        // update MembershipCategory data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipCategoryNumber,MembershipCategoryDescription,MembershipCategoryTotalLoans")] MembershipCategory membershipCategory)
        {
            if (id != membershipCategory.MembershipCategoryNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipCategoryExists(membershipCategory.MembershipCategoryNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(membershipCategory);
        }

        // show MembershipCategory delete view
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipCategory = await _context.MembershipCategories
                .FirstOrDefaultAsync(m => m.MembershipCategoryNumber == id);
            if (membershipCategory == null)
            {
                return NotFound();
            }

            return View(membershipCategory);
        }

        // delete MembershipCategory data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membershipCategory = await _context.MembershipCategories.FindAsync(id);
            _context.MembershipCategories.Remove(membershipCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipCategoryExists(int id)
        {
            return _context.MembershipCategories.Any(e => e.MembershipCategoryNumber == id);
        }
    }
}
