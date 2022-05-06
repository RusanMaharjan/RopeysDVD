using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class Member
{
    public Member()
    {
        Loans = new HashSet<Loan>();
    }
    [Key]
    public int MemberNumber { get; set; }

    public int MembershipCategoryNumber;
    [Required]
    public string MemberLastName { get; set; }
    [Required]
    public string MemberFirstName { get; set; }
    [Required]
    public string MemberAddress { get; set; }
    [Required]
    public DateTime MemberDateOfBirth { get; set; }
    
    [ForeignKey("MembershipCategoryNumber")]
    public MembershipCategory MembershipCategory { get; set; }
    
    public virtual ICollection<Loan> Loans { get; set; }
    
    [NotMapped] 
    public virtual int LoanCount { get; set; }

}