using System.Diagnostics;
using System.Linq;
using Coursework.Data;
using Coursework.Models;
using Coursework.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class MemberLoanController : Controller
{
    private readonly ApplicationDbContext _context;

    public MemberLoanController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Question 8
    //Function to show Loan details of members
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult MemberLoanDetails() {
        List<MemberLoanDetailsDTO> dtos = new List<MemberLoanDetailsDTO>();
        List<Member> memberList = _context.Members.Include(x=>x.MembershipCategory).ToList();
        if (memberList != null) {
            foreach (Member member in memberList)
            {
                var membershipCategory = _context.MembershipCategories.Where(x => x.MembershipCategoryNumber == member.MembershipCategory.MembershipCategoryNumber).First();
                int totalLoan= _context.Loans.Include(x=>x.Member).Where(x => x.Member == member 
                    && x.status == "loaned").ToArray().Length;
                // if (totalLoan > membershipCategory.MembershipCategoryTotalLoans) {
                //     remarks = "Too many DVDs";
                // }
                if (totalLoan > 0)
                {
                    MemberLoanDetailsDTO dto = new MemberLoanDetailsDTO();
                    dto.address = member.MemberAddress;
                    dto.firstName = member.MemberFirstName;
                    dto.lastName = member.MemberLastName;
                    dto.dateOfBirth = member.MemberDateOfBirth;
                    dto.totalLoans = totalLoan;
                    dto.description = membershipCategory.MembershipCategoryDescription;
                    dtos.Add(dto);
                }
                else
                {
                    ViewData["Message"] = "No copies of DVDs are on Loan";
                }
            }
        }
        List<MemberLoanDetailsDTO> orderedDtos= dtos.OrderBy(x=>x.firstName).ToList();
        ViewBag.DTOS = orderedDtos;
        return View(orderedDtos);
    }
    
    //Question 12
    //Function to show details of member who have not taken loan for 31 days
    [Authorize(Roles = "Manager, Assistant")]
    public IActionResult MemberNotTakenLoanFor31Days()
    {
        List<Member> members = new List<Member>();
        members = _context.Members.ToList();
        List<MemberNotTakenLoanDTO> memberNotTakenLoanDtos = new List<MemberNotTakenLoanDTO>();
        List<Loan> loans = new List<Loan>();
        DVDCopy dvdCopy = new DVDCopy();
        Loan loan = new Loan();
        string title = "";
        foreach (var member in members)
        {
            loans = _context.Loans.Include(x=>x.DvdCopy).Where(x=>x.Member == member).ToList();
            var l = loans.Where(x => (DateTime.Now.Date - x.DateOut.Date).TotalDays > 31).ToList();
            foreach (var lo in l)
            {
                dvdCopy = _context.DvdCopies.Include(x=>x.DvdTitle).Where(x=>x.CopyNumber == lo.DvdCopy.CopyNumber).First();
                loan = lo;
                var titles = _context.DvdTitles.Where(x => x.DVDNumber == dvdCopy.DvdTitle.DVDNumber);
                foreach (var dvdTitle in titles)
                {
                    title = dvdTitle.TitleName;
                }
            }

            if (l.Count > 0)
            {
                MemberNotTakenLoanDTO dto = new MemberNotTakenLoanDTO();
                dto.firstName = member.MemberFirstName;
                dto.lastName = member.MemberLastName;
                dto.address = member.MemberAddress;
                dto.DvdTitle = title;
                dto.dateOut = loan.DateOut.Date.ToLongDateString();
                dto.numberOfDays = (DateTime.Now.Date - loan.DateOut).TotalDays;
                memberNotTakenLoanDtos.Add(dto);
            }
        }

        return View(memberNotTakenLoanDtos);
    }
}