using System.ComponentModel;
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
    [DisplayName("Member Number")]
    public int MemberNumber { get; set; }

    public int MembershipCategoryNumber;
    [Required]
    [DisplayName("Member LastName")]
    public string MemberLastName { get; set; }
    [Required]
    [DisplayName("Member FirstName")]
    public string MemberFirstName { get; set; }
    [Required]
    [DisplayName("Member Address")]
    public string MemberAddress { get; set; }
    [Required]
    [DisplayName("Member Date of Birth")]
    public DateTime MemberDateOfBirth { get; set; }
    
    [ForeignKey("MembershipCategoryNumber")]
    public MembershipCategory MembershipCategory { get; set; }
    
    public virtual ICollection<Loan> Loans { get; set; }
    
    [NotMapped] 
    public virtual int LoanCount { get; set; }

}