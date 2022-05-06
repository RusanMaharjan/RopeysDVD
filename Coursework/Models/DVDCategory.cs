using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class DVDCategory
{
    [Key]
    public int CategoryNumber { get; set; }
    [Required]
    public string CategoryDescription { get; set; }
    public Boolean AgeRestricted { get; set; }

}