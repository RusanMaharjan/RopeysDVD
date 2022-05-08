using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class Studio
{
    [Key]
    [DisplayName("Studio Number")]
    public int StudioNumber { get; set; }
    [Required]
    [DisplayName("Studio Name")]
    public string StudioName { get; set; }
}