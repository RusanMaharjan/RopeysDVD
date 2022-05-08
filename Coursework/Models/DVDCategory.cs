using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class DVDCategory
{
    [Key]
    [DisplayName("Category Number")]
    public int CategoryNumber { get; set; }
    [Required]
    [DisplayName("Category Description")]
    public string CategoryDescription { get; set; }
    
    [DisplayName("Age Restricted")]
    public Boolean AgeRestricted { get; set; }

}