using System.Security.Cryptography;
using Coursework.Data;
using Coursework.Models;
using Coursework.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace Coursework.Controllers;

public class DVDTitleController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public DVDTitleController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Question 1
    //Function to show DVD Details by searching actor surname
    [Authorize(Roles = "Manager, Assistant, User")]
    public IActionResult DVDTitleSearchBYActorLastName(String surname)
    {
        var databaseContext = _context.CastMembers.Include(x => x.Actor).Include(x=>x.DvdTitle);
        var actors = from a in databaseContext select a;
        if (!String.IsNullOrEmpty(surname))
        {
            actors = actors.Where(x => x.Actor.ActorSurname.Contains(surname));
        }

        return View(actors.ToList());
    }
    
    //Question 2
    //Function to show DVD on shelves by searching by actor surname
    [Authorize(Roles = "Manager, Assistant, User")]
    public async Task<IActionResult> DvdOnSelves(string lastName)
    {
        var results = from dt in _context.Set<DVDTitle>()
            join castMember in _context.Set<CastMember>()
                on dt.DVDNumber equals castMember.DVDNumber
            join dvdCopy in _context.Set<DVDCopy>()
                    .Where(c => _context.Loans.Any(l => (c.CopyNumber == l.DvdCopy.CopyNumber && l.status == "Returned")))
                on dt.DVDNumber equals dvdCopy.DVDNumber
            join a in _context.Set<Actor>()
                    .Where(x => x.ActorSurname.Contains(lastName))
                on castMember.ActorNumber equals a.ActorNumber
            group new {dt, castMember, dvdCopy} by new {dt.DVDNumber, dt.TitleName, a.ActorSurname}
            into grp
            select new DVDOnSelvesDTO
            {
                DVDNumber = grp.Key.DVDNumber,
                DVDCount = grp.Count(),
                ActorSureName = grp.Key.ActorSurname,
                Title = grp.Key.TitleName,
            };
        ViewData["results"] = results;
        return View();
    }
    
    //Question 3
    // Function to search dvd on loan or not by searching member name
    [Authorize(Roles = "Manager, Assistant")]
    public async Task<IActionResult> SearchDVDByMemberName(string searchString)
    {
        var results = _context.Members.Include(m => m.Loans)
            .ThenInclude(l=>l.DvdCopy)
            .ThenInclude(c=>c.DvdTitle)
            .Where(m=>m.Loans.All(l=>l.DateOut <= DateTime.UtcNow.AddDays(30)))
            .Where(m =>m.MemberFirstName.Contains(searchString)).FirstOrDefault();
        ViewData["member"] = results;
        if(results == null)
        {
            ViewData["loans"] = new List<Loan>();
        }
        else
        {
            ViewData["loans"] = results.Loans;
        }
        return View();
    }

    //Question 4
    //function to show details of dvd
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult DVDDetails()
    {
        var databaseContext = _context.DvdTitles.Include(x => x.Producer).Include(x => x.Studio)
            .OrderBy(x => x.DateReleased).ToList();
        foreach (var data in databaseContext)
        {
            List<string> actorList = _context.CastMembers
                .Where(x => x.DVDNumber == data.DVDNumber)
                .Include(x => x.Actor).OrderBy(x => x.Actor.ActorSurname)
                .Select(x => x.Actor.ActorFirstName + " " + x.Actor.ActorSurname).ToList();
            string actors = string.Join(", ", actorList);
            data.actors = actors;
        }

        return View(databaseContext);
    }
    
    //Question 7
    //function to get data of Returned DVD i.e: Show dvd data which is not in loan
    [Authorize(Roles = "Manager, Assistant")]
    public async Task<IActionResult> ReturnDVD(int id)
    {
        Loan loan = _context.Loans.Where(x=>x.CopyNumber == id).Include(x=>x.DvdCopy).ThenInclude(x=>x.DvdTitle).FirstOrDefault();
        if (loan != null)
        {
            if (loan.status == "Loaned")
            {
                loan.DateReturned = DateTime.Now;
                _context.Update(loan);
                await _context.SaveChangesAsync();
                TimeSpan? days = loan.DateReturned - loan.DateDue;
                if (days.Value.Days > 0)
                {
                    ViewData["message"] ="Dvd returned successfully your total cost is "+ days.Value.Days * loan.DvdCopy.DvdTitle.PenaltyCharge ;
                    return View();
                }
                ViewData["message"] = "Dvd returned successfully before due date";
                return View();
            }
            else
            {
                ViewData["message"] = "Dvd already returned";
                return View();
            }
        }

        ViewData["message"] = "DVD doesnot exist!!";
        return View();
    }
    
    //Question 10
    //Function to show list of DVD which are not on loan
    [Authorize(Roles = "Manager, Assistant")]
    public List<DVDCopy> DVDNotOnLoan()
    {
        List<DVDCopy> dvdCopies = _context.DvdCopies.Include(x => x.DvdTitle).ToList();
        List<DVDCopy> newCopies = dvdCopies.Where(x=>(DateTime.Now.Date - x.DatePurchased.Date).TotalDays >= 365).ToList();
        List<DVDCopy> dvdNotInLoan = new List<DVDCopy>();
        foreach (var copy in newCopies)
        {
            List<Loan> copyLoans = _context.Loans.Where(x=>x.DvdCopy == copy && x.status == "Loaned").ToList();
            if (copyLoans.Count == 0)
            {
                dvdNotInLoan.Add(copy);
            }
        }

        return dvdNotInLoan;
    }

    // Functions to show DVDs older than 365 days
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult DVDOlderThan365Days()
    {
        List<DVDCopy> dvdCopies = DVDNotOnLoan();
        return View(dvdCopies);
    }

    // Functions to show list of 365 days old dvd to be deleted
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult deleteDVDOlderThan365Days()
    {
        List<DVDCopy> dvdCopyNotInLoan = DVDNotOnLoan();
        foreach (var dvdCopy in dvdCopyNotInLoan)
        {
            try
            {
                var copy_data = _context.DvdCopies.Where(x=>x.CopyNumber == dvdCopy.CopyNumber).First();
                _context.DvdCopies.Remove(copy_data);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ViewBag.DeleteMessage = "Copy of DVD has been deleted";
        return Redirect(null);
    }

    //Question 13
    //Function to show list of DVD which are not on loan for 31 days
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult DVDNotLoanFor31Days()
    {
        List<DVDTitle> dvdTitles = _context.DvdTitles.Include(x=>x.Producer).Include(x=>x.Studio).Include(x=>x.DvdCategory).ToList();
        List<DVDTitleDTO> dtos = new List<DVDTitleDTO>();
        List<Loan> copyLoans = new List<Loan>();
        List<Loan> copyLoansForLast31Days = new List<Loan>();
        bool count = false;
        foreach (var dvdTitle in dvdTitles)
        {
            var dvdCopies = _context.DvdCopies.Include(x=>x.DvdTitle).Where(x=>x.DvdTitle == dvdTitle).ToList();
            foreach (DVDCopy copy in dvdCopies)
            {
                copyLoans = _context.Loans.Include(x=>x.DvdCopy).Where(x=>x.DvdCopy == copy && x.status == "Loaned").ToList();
                copyLoansForLast31Days.Where(x => (DateTime.Now.Date - x.DateOut.Date).TotalDays <= 31).ToList();
                if (copyLoansForLast31Days.Count > 0)
                {
                    count = true;
                    break;
                }
            }

            if (count == false)
            {
                DVDTitleDTO dto = new DVDTitleDTO();
                dto.Title = dvdTitle.TitleName;
                dto.AgeRestriction = dvdTitle.DvdCategory.AgeRestricted.ToString();
                dto.Description = dvdTitle.DvdCategory.CategoryDescription;
                dto.ProducerName = dvdTitle.Producer.ProducerName;
                dto.releasedDate = dvdTitle.DateReleased;
                dto.SudioName = dvdTitle.Studio.StudioName;
                dtos.Add(dto);
            }

            count = false;
        }

        return View(dtos);
    }
    
    //Question 9
    // GET DVDTitles create view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult CreateDVD()
    {
        ViewData["CategoryNumber"] = new SelectList(_context.DvdCategories, "CategoryNumber", "CategoryDescription");
        ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerName");
        ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioName");
        return View();
    }

    // POST: DVDTitles data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    public IActionResult CreateDVD(int CategoryNumber, int StudioNumber, int ProducerNumber, string titleName , string DateReleased, int StandardCharge, int PenaltyCharge, DVDTitle dVDTitle)
    {
        dVDTitle.CategoryNumber = CategoryNumber;
        dVDTitle.ProducerNumber = ProducerNumber;
        dVDTitle.StudioNumber = StudioNumber;
        dVDTitle.TitleName = titleName;
        dVDTitle.StandardCharge = StandardCharge;
        dVDTitle.PenaltyCharge = PenaltyCharge;
        dVDTitle.DateReleased = DateReleased;
        try
        {
            _context.Add(dVDTitle);
            _context.SaveChanges();
            TempData["success"] = "DVD Title Added Successfully.";
            return RedirectToAction("DVDDetails");
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    // Get DVD title edit view
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var dvdTitle = _context.DvdTitles.Find(id);
        if (dvdTitle == null)
        {
            return NotFound();
        }
        ViewData["CategoryNumber"] = new SelectList(_context.DvdCategories, "CategoryNumber", "CategoryDescription", dvdTitle.CategoryNumber);
        ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerName", dvdTitle.ProducerNumber);
        ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioName", dvdTitle.StudioNumber);
        return View(dvdTitle);
    }
    
    //Update dvd title data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int CategoryNumber, int StudioNumber, int ProducerNumber, string titleName , string DateReleased, int StandardCharge, int PenaltyCharge, DVDTitle dVDTitle)
    {
        dVDTitle.CategoryNumber = CategoryNumber;
        dVDTitle.ProducerNumber = ProducerNumber;
        dVDTitle.StudioNumber = StudioNumber;
        dVDTitle.TitleName = titleName;
        dVDTitle.StandardCharge = StandardCharge;
        dVDTitle.PenaltyCharge = PenaltyCharge;
        dVDTitle.DateReleased = DateReleased;
        try
        {
            _context.Update(dVDTitle);
            _context.SaveChanges();
            TempData["update"] = "DVD Title Updated Successfully.";
            return RedirectToAction("DVDDetails");
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    // GET DVDTitles delete view
    [Authorize(Roles = "Manager, Assistant")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dVDTitle = await _context.DvdTitles
            .Include(d => d.DvdCategory)
            .Include(d => d.Producer)
            .Include(d => d.Studio)
            .FirstOrDefaultAsync(m => m.DVDNumber == id);
        if (dVDTitle == null)
        {
            return NotFound();
        }

        return View(dVDTitle);
    }

    // Delete DVDTitles data
    [Authorize(Roles = "Manager, Assistant")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dVDTitle = await _context.DvdTitles.FindAsync(id);
        _context.DvdTitles.Remove(dVDTitle);
        await _context.SaveChangesAsync();
        TempData["delete"] = "DVD Title Deleted Successfully.";
        return RedirectToAction("DVDDetails");
    }

    private bool DVDTitleExists(int id)
    {
        return _context.DvdTitles.Any(e => e.DVDNumber == id);
    }
}
