using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class LoanTypeController : Controller
{
    private ApplicationDbContext _context;

    public LoanTypeController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET loan type details
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult Index()
    {
        IEnumerable<LoanType> loanTypes = _context.LoanTypes;
        return View(loanTypes);
    }

    // show loan type create view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //Store loan type data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(LoanType loanType)
    {
        if (ModelState.IsValid)
        {
            _context.LoanTypes.Add(loanType);
            _context.SaveChanges();
            TempData["success"] = "Loan Type Added Successfully.";
            return RedirectToAction("Index");
        }

        return View(loanType);
    }

    // show loan type edit view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var loanType = _context.LoanTypes.Find(id);
        if (loanType == null)
        {
            return NotFound();
        }

        return View(loanType);
    }

    //Update loan type data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(LoanType l)
    {
        if (ModelState.IsValid)
        {
            _context.LoanTypes.Update(l);
            _context.SaveChanges();
            TempData["update"] = "Loan Type Updated Successfully.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    // show loan type delete view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var loanType = _context.LoanTypes.Find(id);
        if (loanType == null)
        {
            return NotFound();
        }

        return View(loanType);
    }

    //Delete loan type data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var loanType = _context.LoanTypes.Find(id);
        if (loanType == null)
        {
            return NotFound();
        }

        _context.LoanTypes.Remove(loanType);
        _context.SaveChanges();
        TempData["delete"] = "Loan Type Deleted Successfully";
        return RedirectToAction("Index");
    }
}