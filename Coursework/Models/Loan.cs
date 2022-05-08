using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class Loan
{
    [Key]
    [DisplayName("Loan Number")]
    public int LoanNumber { get; set; }
    
    [DisplayName("Loan Type Number")]
    public int LoanTypeNumber { get; set; }
    
    [DisplayName("Copy Number")]
    public int CopyNumber { get; set; }
    
    [DisplayName("Member Number")]
    public int MemberNumber { get; set; }

    [DisplayName("Date Out")]
    public DateTime DateOut { get; set; }
    
    [DisplayName("Date Due")]
    public DateTime DateDue { get; set; }
    
    [DisplayName("Date Returned")]
    public DateTime? DateReturned { get; set; }
    
    [DisplayName("Status")]
    public string status { get; set; }
    
    [ForeignKey("LoanTypeNumber")]
    public LoanType LoanType { get; set; }
    
    [ForeignKey("CopyNumber")]
    public DVDCopy DvdCopy { get; set; }
    
    [ForeignKey("MemberNumber")]
    public Member Member { get; set; }
}