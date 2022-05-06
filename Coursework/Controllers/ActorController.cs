using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class ActorController : Controller
{
    private ApplicationDbContext _context;

    public ActorController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET All Actors
    //show all actors
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult Index()
    {
        IEnumerable<Actor> actors = _context.Actors;
        return View(actors);
    }
    
    //get actor create view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [Authorize(Roles = "Manager, Assistant")]
    //post actor data to database
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Actor actor)
    {
        if (ModelState.IsValid)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
            TempData["success"] = "Actor Created Successfully.";
            return RedirectToAction("Index");
        }

        return View(actor);
    }
    
    //get actor edit view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var actor = _context.Actors.Find(id);
        if (actor == null)
        {
            return NotFound();
        }
        return View(actor);
    }
    
    //update actor data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Actor a)
    {
        if (ModelState.IsValid)
        {
            _context.Actors.Update(a);
            _context.SaveChanges();
            TempData["success"] = "Actor Updated Successfully.";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    
    //delete actor view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var actor = _context.Actors.Find(id);
        if (actor == null)
        {
            return NotFound();
        }
        return View(actor);
    }
    
    //Delete actor data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var actor = _context.Actors.Find(id);
        if (actor==null)
        {
            return NotFound();
        }

        _context.Actors.Remove(actor);
        _context.SaveChanges();
        TempData["delete"] = "Actor Deleted Successfully.";
        return RedirectToAction("Index");
    }
}