#nullable disable
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
    public class DVDCopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DVDCopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // show DVDCopies data
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DvdCopies.Include(d => d.DvdTitle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET DVDCopies Create view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Create()
        {
            ViewData["DVDNumber"] = new SelectList(_context.DvdTitles, "DVDNumber", "TitleName");
            return View();
        }

        // POST DVDCopies data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int DVDNumber, DateTime DatePurchased, DVDCopy dVDCopy)
        {
            dVDCopy.DVDNumber = DVDNumber;
            dVDCopy.DatePurchased = DatePurchased;
            try
            {
                _context.Add(dVDCopy);
                _context.SaveChanges();
                TempData["success"] = "DVD Copy Added Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET DVDCopies edit view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var dVDCopy = _context.DvdCopies.Find(id);
            if (dVDCopy == null)
            {
                return NotFound();
            }
            ViewData["DVDNumber"] = new SelectList(_context.DvdTitles, "DVDNumber", "TitleName", dVDCopy.DVDNumber);
            return View(dVDCopy);
        }

        // Update DVDCopies edit data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int DVDNumber, DateTime DatePurchased, DVDCopy dVDCopy)
        {
            dVDCopy.DVDNumber = DVDNumber;
            dVDCopy.DatePurchased = DatePurchased;
            try
            {
                _context.Update(dVDCopy);
                _context.SaveChanges();
                TempData["update"] = "DVD Copy Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET DVDCopies delete view
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DvdCopies
                .Include(d => d.DvdTitle)
                .FirstOrDefaultAsync(m => m.CopyNumber == id);
            if (dVDCopy == null)
            {
                return NotFound();
            }

            return View(dVDCopy);
        }

        // Delete DVDCopies data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dVDCopy = await _context.DvdCopies.FindAsync(id);
            _context.DvdCopies.Remove(dVDCopy);
            await _context.SaveChangesAsync();
            TempData["delete"] = "DVD Copy Deleted Successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool DVDCopyExists(int id)
        {
            return _context.DvdCopies.Any(e => e.CopyNumber == id);
        }
    }
}
