using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class ProducerController : Controller
{
     private ApplicationDbContext _context;

    public ProducerController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // show All producer details
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult Index()
    {
        IEnumerable<Producer> producers = _context.Producers;
        return View(producers);
    }
    
    // show producer create view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    // store producer data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Producer producer)
    {
        if (ModelState.IsValid)
        {
            _context.Producers.Add(producer);
            _context.SaveChanges();
            TempData["success"] = "Producer Added Successfully.";
            return RedirectToAction("Index");
        }

        return View(producer);
    }
    
    // Show edit producer view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var producer = _context.Producers.Find(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }
    
    // update producer data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Producer p)
    {
        if (ModelState.IsValid)
        {
            _context.Producers.Update(p);
            _context.SaveChanges();
            TempData["update"] = "Producer Updated Successfully.";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    
    //show producer delete view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var producer = _context.Producers.Find(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }
    
    // delete producer data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var producer = _context.Producers.Find(id);
        if (producer==null)
        {
            return NotFound();
        }

        _context.Producers.Remove(producer);
        _context.SaveChanges();
        TempData["delete"] = "Producer Deleted Successfully.";
        return RedirectToAction("Index");
    }
}