using Coursework.Data;
using Coursework.Models;
using Coursework.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class DVDCopyController : Controller
{
    private ApplicationDbContext _context;

    public DVDCopyController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //Question 11
    // Function for DVD copies on loan
    // It shows Copies of DVD on loan
    [Authorize(Roles = "Manager, Assistant")]
    [HttpGet]
    public IActionResult DVDCopiesOnLoan()
    {
        List<DVDCopiesLoanDTO> dvdCopiesLoanDtos = new List<DVDCopiesLoanDTO>(); //DVDCopiesLoanDTO object
        List<DVDTitle> dvdTitles = _context.DvdTitles.ToList(); // converting data of dvd title to list
        List<DVDCopy> dvdCopies = new List<DVDCopy>(); // DVD copy object
        List<Loan> loans = new List<Loan>(); // Loan object
        Member member = new Member(); // Member object
        foreach (var dvdTitle in dvdTitles) // foreach loop to show dvd copies loan
        {
            dvdCopies = _context.DvdCopies.Include(x=>x.DvdTitle).Where(x=>x.DvdTitle == dvdTitle).ToList();
            foreach (var dvdCopy in dvdCopies)
            {
                loans = _context.Loans.Include(x=>x.DvdCopy).Include(x=>x.Member).Where(x=>x.DvdCopy == dvdCopy && x.status=="Loaned").ToList();
                if (loans != null)
                {
                    foreach (var loan in loans)
                    {
                        member = _context.Members.Where(x => x.MemberNumber == loan.Member.MemberNumber).First();
                        DVDCopiesLoanDTO copiesLoanDto = new DVDCopiesLoanDTO();
                        copiesLoanDto.dateOut = loan.DateOut;
                        copiesLoanDto.title = dvdTitle.TitleName;
                        copiesLoanDto.name = member.MemberFirstName + "" + member.MemberLastName;
                        copiesLoanDto.copyNumber = dvdCopy.CopyNumber;
                        dvdCopiesLoanDtos.Add(copiesLoanDto);
                    }
                }
            }
        }

        dvdCopiesLoanDtos.OrderBy(x=>x.dateOut).ThenBy(x=>x.title);
        return View(dvdCopiesLoanDtos);
    }
}