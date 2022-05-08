using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class MembershipCategory
{
    [Key]
    [DisplayName("Membership Category Number")]
    public int MembershipCategoryNumber { get; set; }
    [Required]
    [DisplayName("Membership Category Description")]
    public string MembershipCategoryDescription { get; set; }
    [Required]
    [DisplayName("Membership Category Total Loans")]
    public int MembershipCategoryTotalLoans { get; set; }
}   