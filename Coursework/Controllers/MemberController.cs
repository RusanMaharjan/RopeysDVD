using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class MemberController : Controller
{
    private readonly ApplicationDbContext _context;

    public MemberController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET Members details
    [Authorize(Roles = "Manager, Assistant")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Members.ToListAsync());
    }
    
    // show member create view
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult Create()
    {
        ViewData["MembershipCategoryNumber"] = new SelectList(_context.MembershipCategories, "MembershipCategoryNumber", "MembershipCategoryDescription");
        return View();
    }

    // show Members Create view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int MembershipCategoryNumber, string MemberLastName, string MemberFirstName, string MemberAddress, DateTime MemberDateOfBirth, Member member)
    {
        member.MembershipCategoryNumber = MembershipCategoryNumber;
        member.MemberLastName = MemberLastName;
        member.MemberFirstName = MemberFirstName;
        member.MemberAddress = MemberAddress;
        member.MemberDateOfBirth = MemberDateOfBirth;
        try
        {
            _context.Add(member);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    // show edit member veiw
    [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var member = _context.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["MembershipCategoryNumber"] = new SelectList(_context.MembershipCategories, "MembershipCategoryNumber", "MembershipCategoryDescription", member.MembershipCategory);
            return View(member);
        }

        // update member data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int MembershipCategoryNumber, string MemberLastName, string MemberFirstName, string MemberAddress, DateTime MemberDateOfBirth, Member member)
        {
            member.MembershipCategoryNumber = MembershipCategoryNumber;
            member.MemberLastName = MemberLastName;
            member.MemberFirstName = MemberFirstName;
            member.MemberAddress = MemberAddress;
            member.MemberDateOfBirth = MemberDateOfBirth;
            try
            {
                _context.Update(member);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        // show member delete view 
        [Authorize(Roles = "Manager, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(l => l.MembershipCategory)
                .FirstOrDefaultAsync(m => m.MemberNumber == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // Delete member data
        [Authorize(Roles = "Manager, Assistant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
}