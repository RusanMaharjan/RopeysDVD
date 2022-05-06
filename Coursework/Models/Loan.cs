using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class Loan
{
    [Key]
    public int LoanNumber { get; set; }
    public int LoanTypeNumber { get; set; }
    public int CopyNumber { get; set; }
    public int MemberNumber { get; set; }

    public DateTime DateOut { get; set; }
    public DateTime DateDue { get; set; }
    public DateTime? DateReturned { get; set; }
    
    public string status { get; set; }
    
    [ForeignKey("LoanTypeNumber")]
    public LoanType LoanType { get; set; }
    
    [ForeignKey("CopyNumber")]
    public DVDCopy DvdCopy { get; set; }
    
    [ForeignKey("MemberNumber")]
    public Member Member { get; set; }
}