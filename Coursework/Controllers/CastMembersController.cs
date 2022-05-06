using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class CastMembersController : Controller
{
    private readonly ApplicationDbContext _context;

        public CastMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastMembers
        //show cast memebers data
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CastMembers.Include(c => c.Actor).Include(c => c.DvdTitle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CastMembers/Create
        //show cast member create view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Create()
        {
            ViewData["ActorNumber"] = new SelectList(_context.Actors, "ActorNumber", "ActorFirstName");
            ViewData["DVDNumber"] = new SelectList(_context.DvdTitles, "DVDNumber", "TitleName");
            return View();
        }

        // POST CastMembers data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int DVDNumber,int ActorNumber, CastMember castMember)
        {
            castMember.DVDNumber = DVDNumber;
            castMember.ActorNumber = ActorNumber;
            try
            {
                _context.Add(castMember);
                _context.SaveChanges();
                TempData["success"] = "Cast Member Added Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET CastMembers edit view
        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var castMember = _context.CastMembers.Find(id);
            if (castMember == null)
            {
                return NotFound();
            }
            ViewData["ActorNumber"] = new SelectList(_context.Actors, "ActorNumber", "ActorFirstName", castMember.ActorNumber);
            ViewData["DVDNumber"] = new SelectList(_context.DvdTitles, "DVDNumber", "TitleName", castMember.DVDNumber);
            return View(castMember);
        }

        // update CastMembers edit data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int DVDNumber, int ActorNumber, CastMember castMember)
        {
            castMember.DVDNumber = DVDNumber;
            castMember.ActorNumber = ActorNumber;
            try
            {
                _context.Update(castMember);
                _context.SaveChanges();
                TempData["update"] = "Cast Member Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET CastMembers delete view
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castMember = await _context.CastMembers
                .Include(c => c.Actor)
                .Include(c => c.DvdTitle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castMember == null)
            {
                return NotFound();
            }

            return View(castMember);
        }

        // delete CastMembers data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var castMember = await _context.CastMembers.FindAsync(id);
            _context.CastMembers.Remove(castMember);
            await _context.SaveChangesAsync();
            TempData["delete"] = "Cast Member Deleted Successfully.";
            return RedirectToAction("Index");
        }

        private bool CastMemberExists(int id)
        {
            return _context.CastMembers.Any(e => e.Id == id);
        }
}