using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class MembershipCategory
{
    [Key]
    public int MembershipCategoryNumber { get; set; }
    [Required]
    public string MembershipCategoryDescription { get; set; }
    [Required]
    public int MembershipCategoryTotalLoans { get; set; }
}   